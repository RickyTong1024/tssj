#ifndef __INTERFACE_H__
#define __INTERFACE_H__

#include <string>
#include <boost/function.hpp>
#include <google/protobuf/message.h>
#include <ace/Time_Value.h>
#include <ace/Event_Handler.h>

typedef boost::function<void (const std::string &data)> ResponseFunc;
typedef boost::function<int(const ACE_Time_Value & cur_time)> Callback;

#define BEGIN_PACKET_MAP \
	virtual void dispath_packet_handle(Packet *packet) { \
	switch (packet->opcode()) {

#define PACKET_HANDLER(opcode, handler) \
	case opcode: \
		handler(packet); \
		break;

#define END_PACKET_MAP \
} \
}

#define BEGIN_PUSH_MAP \
	virtual void dispath_push_handle(int opcode, const std::string &data, const std::string &name) { \
	switch(opcode) {

#define PUSH_HANDLER(opcode, handler) \
	case opcode: \
		handler(data, name); \
		break;

#define END_PUSH_MAP \
} \
}

#define BEGIN_REQ_MAP \
	virtual void dispath_req_handle(int opcode, const std::string &data, const std::string &name, int id) { \
	switch(opcode) {

#define REQ_HANDLER(opcode, handler) \
	case opcode: \
		handler(data, name, id); \
		break;

#define END_REQ_MAP \
} \
}


enum opcmd_t {
	opc_insert = 1,   /// 插入实体
	opc_query = 2,   /// 查询实体 
	opc_update = 3,   /// 更新实体 
	opc_remove = 4,   /// 删除实体 
};

class Request
{
public:
	Request(void)
		: op_(opc_insert)
		, guid_(0)
		, data_(0)
		, suc_(false)
	{

	}

	~Request(void)
	{
		if (data_)
		{
			delete data_;
			data_ = 0;
		}
	}

	void add(opcmd_t op, uint64_t guid, google::protobuf::Message *data)
	{
		op_ = op;
		guid_ = guid;
		data_ = data;
	}

	void set_success(bool suc)
	{
		suc_ = suc;
	}

	opcmd_t op() const { return op_; }

	uint64_t guid() const { return guid_; }

	google::protobuf::Message *data() const { return data_; }

	google::protobuf::Message *release_data() { google::protobuf::Message *data = data_; data_ = 0; return data; }

	bool success() const { return suc_; }

private:
	opcmd_t op_;
	uint64_t guid_;
	google::protobuf::Message *data_;
	bool suc_;
};

typedef boost::function<void(Request *)> Upcaller;

struct NameStr
{
	NameStr() : name(""), suc(false) {}

	bool success() const { return suc; }

	std::string name;
	std::vector<uint64_t> guid;
	bool suc;
};

typedef boost::function<void(NameStr *)> Namecaller;

struct TermInfo
{
	std::string username;
	std::string password;
	int lang_ver;
	std::string serverid;
	std::string extra;
	int gm_level;
	std::string sig;
	uint64_t time;
	uint64_t cztime;
	int pck_id;
	std::string last_pck;
	std::string device;
	int version;
	uint64_t device_time;
	std::string platform;
	uint64_t gag_time;

	TermInfo() : username(""), serverid(""), extra(""), gm_level(0), sig(""), pck_id(1), last_pck(""), lang_ver(0){}
};

class Packet;
class DBCFile;

namespace mmg {

	class GameInterface
	{
	public:
		virtual void dispath_push_handle(int opcode, const std::string &data, const std::string &name) = 0;

		virtual void dispath_req_handle(int opcode, const std::string &data, const std::string &name, int id) = 0;

		virtual void dispath_packet_handle(Packet *packet) = 0;
	};

	class RpcService
	{
	public:
		virtual void request(const std::string &name, int opcode, const std::string &msg, ResponseFunc func) = 0;

		virtual void push(const std::string &name, int opcode, const std::string &msg) = 0;

		virtual void response(const std::string &name, int id, const std::string &msg, int error_code = 0, const std::string &error_text = "") = 0;

		virtual void self_request(int opcode, const std::string &msg, ResponseFunc func) = 0;
	};

	class Timer
	{
	public:
		virtual int start() = 0;

		virtual int schedule(Callback task, int expire, const std::string &name) = 0;

		virtual int cancel(int expiry_id) = 0;

		virtual uint64_t now(void) = 0;

		virtual int hour(void) = 0;

		virtual int weekday(void) = 0;

		virtual int day(void) = 0;

		virtual int month(void) = 0;

		virtual bool trigger_time(uint64_t old_time, int hour, int minute) = 0;

		virtual bool trigger_week_time(uint64_t old_time) = 0;

		virtual bool trigger_month_time(uint64_t old_time) = 0;

		virtual int run_day(uint64_t old_time) = 0;
	};

	class Env
	{
	public:
		virtual std::string get_master_value(const std::string &key) = 0;

		virtual std::string get_id_value(const std::string &name, const std::string &key) = 0;

		virtual void get_server_names(const std::string &kind, std::vector<std::string> &names) = 0;

		virtual void get_server_kinds(std::vector<std::string> &kinds) = 0;

		virtual std::string get_db_value(const std::string &key) = 0;

		virtual std::string get_game_value(const std::string &key) = 0;

		virtual std::string get_server_state(const std::string &state) = 0;
	};

	class TcpService
	{
	public:
		virtual void send_msg(int hid, Packet *pck) = 0;

		virtual void destory(int hid) = 0;
	};

	class GTool
	{
	public:
		virtual uint64_t assign(int et) = 0;

		virtual uint64_t start_time() = 0;

		virtual int random_start_day() = 0;
	};

	class Pool
	{
	public:
		enum estatus
		{
			state_none,
			state_new,
			state_update,
		};

		template<typename OBJECT>
		OBJECT * object(uint64_t guid)
		{
			return dynamic_cast<OBJECT*>(get(guid));
		}

		/// 添加一个新的实体对象到缓存中
		virtual int add(uint64_t guid, google::protobuf::Message *entity, estatus es) = 0;

		/// 删除实体对象, ref_guid 代表了被删除的实体对象是被那个实体所参考, 为0代表没有被参考, ref_guid不为0时将在ref_guid被更新时更新
		virtual int remove(uint64_t guid, uint64_t ref_guid) = 0;

		/// 删除ref_guid相关的实体对象
		virtual int remove_ref(uint64_t ref_guid) = 0;

		/// 从数据池中释放指定的实体对象, 这意味着不再受到托管
		virtual google::protobuf::Message * release(uint64_t guid) = 0;

		/// 从缓存中获取实体
		virtual google::protobuf::Message * get(uint64_t guid) = 0;

		/// 查询实体的当前状态
		virtual estatus get_state(uint64_t guid) = 0;

		/// 设置实体当前状态
		virtual void set_state(uint64_t guid, estatus es) = 0;

		virtual void get_entitys(int type, std::vector<uint64_t> &guids) = 0;

		virtual bool full() = 0;

		virtual int upcall(Request *req, Upcaller caller) = 0;

		virtual int doupcall(void) = 0;

		virtual int namecall(NameStr *name, Namecaller caller) = 0;

		virtual int donamecall(void) = 0;
	};

	class Channel
	{
	public:
		virtual void add_channel(uint64_t guid, TermInfo ti, bool is_login) = 0;

		//virtual void update_channel(uint64_t guid, uint64_t gag_time) = 0;

		virtual TermInfo * get_channel(uint64_t guid) = 0;

		virtual void del_channel(uint64_t guid) = 0;

		virtual void refresh_offline_time(uint64_t guid) = 0;

		virtual int check_sig(uint64_t guid, const std::string &sig) = 0;

		virtual int get_channel_num() = 0;
		virtual int get_channel_lang(uint64_t guid) = 0;

		virtual int get_real_channel_num() = 0;

		virtual bool online(uint64_t guid) = 0;

		virtual int check_pck(uint64_t guid, int pck_id) = 0;

		virtual void last_pck(uint64_t guid, std::string &pck) = 0;

		virtual void set_last_pck(uint64_t guid, const std::string &pck) = 0;
	};

	class GameService
	{
	public:
		virtual int add_msg(Packet *pck) = 0;
	};

	class Scheme
	{
	public:
		virtual DBCFile * get_dbc(const std::string &name, bool reset = false) = 0;

		virtual int read_file(const std::string &name, std::string &res, bool reset = false) = 0;

		virtual int search_illword(const std::string& text,bool can_space, bool can_return) = 0;

		virtual void change_illword(std::string &text) = 0;

		virtual int get_server_str(int lang, std::string &str, const char * sstr, ...) = 0;

		virtual std::string get_lang_str(int lang, const std::string& str) = 0;

		virtual bool valid_name(const std::string& text) = 0;
	};

	class Log
	{
	public:
		virtual int debug(const char *text, ...) = 0;

		virtual int error(const char *text, ...) = 0;

		virtual void log(const std::string &username,
			const std::string &server_id,
			uint64_t player_guid,
			int type,
			int value1,
			int value2,
			int value3,
			int value4,
			const std::string &platform) = 0;
	};
}

#endif // !__RPC_INTERFACE_H__

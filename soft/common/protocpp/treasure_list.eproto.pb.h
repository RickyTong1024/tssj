// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: treasure_list.eproto

#ifndef PROTOBUF_INCLUDED_treasure_5flist_2eeproto
#define PROTOBUF_INCLUDED_treasure_5flist_2eeproto

#include <string>

#include <google/protobuf/stubs/common.h>

#if GOOGLE_PROTOBUF_VERSION < 3006001
#error This file was generated by a newer version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please update
#error your headers.
#endif
#if 3006001 < GOOGLE_PROTOBUF_MIN_PROTOC_VERSION
#error This file was generated by an older version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please
#error regenerate this file with a newer version of protoc.
#endif

#include <google/protobuf/io/coded_stream.h>
#include <google/protobuf/arena.h>
#include <google/protobuf/arenastring.h>
#include <google/protobuf/generated_message_table_driven.h>
#include <google/protobuf/generated_message_util.h>
#include <google/protobuf/inlined_string_field.h>
#include <google/protobuf/metadata.h>
#include <google/protobuf/message.h>
#include <google/protobuf/repeated_field.h>  // IWYU pragma: export
#include <google/protobuf/extension_set.h>  // IWYU pragma: export
#include <google/protobuf/unknown_field_set.h>
// @@protoc_insertion_point(includes)
#define PROTOBUF_INTERNAL_EXPORT_protobuf_treasure_5flist_2eeproto 

namespace protobuf_treasure_5flist_2eeproto {
// Internal implementation detail -- do not use these members.
struct TableStruct {
  static const ::google::protobuf::internal::ParseTableField entries[];
  static const ::google::protobuf::internal::AuxillaryParseTableField aux[];
  static const ::google::protobuf::internal::ParseTable schema[1];
  static const ::google::protobuf::internal::FieldMetadata field_metadata[];
  static const ::google::protobuf::internal::SerializationTable serialization_table[];
  static const ::google::protobuf::uint32 offsets[];
};
void AddDescriptors();
}  // namespace protobuf_treasure_5flist_2eeproto
namespace dhc {
class treasure_list_t;
class treasure_list_tDefaultTypeInternal;
extern treasure_list_tDefaultTypeInternal _treasure_list_t_default_instance_;
}  // namespace dhc
namespace google {
namespace protobuf {
template<> ::dhc::treasure_list_t* Arena::CreateMaybeMessage<::dhc::treasure_list_t>(Arena*);
}  // namespace protobuf
}  // namespace google
namespace dhc {

// ===================================================================

class treasure_list_t : public ::google::protobuf::Message /* @@protoc_insertion_point(class_definition:dhc.treasure_list_t) */ {
 public:
  treasure_list_t();
  virtual ~treasure_list_t();

  treasure_list_t(const treasure_list_t& from);

  inline treasure_list_t& operator=(const treasure_list_t& from) {
    CopyFrom(from);
    return *this;
  }
  #if LANG_CXX11
  treasure_list_t(treasure_list_t&& from) noexcept
    : treasure_list_t() {
    *this = ::std::move(from);
  }

  inline treasure_list_t& operator=(treasure_list_t&& from) noexcept {
    if (GetArenaNoVirtual() == from.GetArenaNoVirtual()) {
      if (this != &from) InternalSwap(&from);
    } else {
      CopyFrom(from);
    }
    return *this;
  }
  #endif
  static const ::google::protobuf::Descriptor* descriptor();
  static const treasure_list_t& default_instance();

  static void InitAsDefaultInstance();  // FOR INTERNAL USE ONLY
  static inline const treasure_list_t* internal_default_instance() {
    return reinterpret_cast<const treasure_list_t*>(
               &_treasure_list_t_default_instance_);
  }
  static constexpr int kIndexInFileMessages =
    0;

  void Swap(treasure_list_t* other);
  friend void swap(treasure_list_t& a, treasure_list_t& b) {
    a.Swap(&b);
  }

  // implements Message ----------------------------------------------

  inline treasure_list_t* New() const final {
    return CreateMaybeMessage<treasure_list_t>(NULL);
  }

  treasure_list_t* New(::google::protobuf::Arena* arena) const final {
    return CreateMaybeMessage<treasure_list_t>(arena);
  }
  void CopyFrom(const ::google::protobuf::Message& from) final;
  void MergeFrom(const ::google::protobuf::Message& from) final;
  void CopyFrom(const treasure_list_t& from);
  void MergeFrom(const treasure_list_t& from);
  void Clear() final;
  bool IsInitialized() const final;

  size_t ByteSizeLong() const final;
  bool MergePartialFromCodedStream(
      ::google::protobuf::io::CodedInputStream* input) final;
  void SerializeWithCachedSizes(
      ::google::protobuf::io::CodedOutputStream* output) const final;
  ::google::protobuf::uint8* InternalSerializeWithCachedSizesToArray(
      bool deterministic, ::google::protobuf::uint8* target) const final;
  int GetCachedSize() const final { return _cached_size_.Get(); }

  private:
  void SharedCtor();
  void SharedDtor();
  void SetCachedSize(int size) const final;
  void InternalSwap(treasure_list_t* other);
  private:
  inline ::google::protobuf::Arena* GetArenaNoVirtual() const {
    return NULL;
  }
  inline void* MaybeArenaPtr() const {
    return NULL;
  }
  public:

  ::google::protobuf::Metadata GetMetadata() const final;

  // nested types ----------------------------------------------------

  // accessors -------------------------------------------------------

  // repeated int32 nalflag = 4;
  int nalflag_size() const;
  void clear_nalflag();
  static const int kNalflagFieldNumber = 4;
  ::google::protobuf::int32 nalflag(int index) const;
  void set_nalflag(int index, ::google::protobuf::int32 value);
  void add_nalflag(::google::protobuf::int32 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
      nalflag() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
      mutable_nalflag();

  // repeated uint64 player_guid = 5;
  int player_guid_size() const;
  void clear_player_guid();
  static const int kPlayerGuidFieldNumber = 5;
  ::google::protobuf::uint64 player_guid(int index) const;
  void set_player_guid(int index, ::google::protobuf::uint64 value);
  void add_player_guid(::google::protobuf::uint64 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::uint64 >&
      player_guid() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::uint64 >*
      mutable_player_guid();

  // repeated int32 player_template = 6;
  int player_template_size() const;
  void clear_player_template();
  static const int kPlayerTemplateFieldNumber = 6;
  ::google::protobuf::int32 player_template(int index) const;
  void set_player_template(int index, ::google::protobuf::int32 value);
  void add_player_template(::google::protobuf::int32 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
      player_template() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
      mutable_player_template();

  // repeated bytes player_name = 7;
  int player_name_size() const;
  void clear_player_name();
  static const int kPlayerNameFieldNumber = 7;
  const ::std::string& player_name(int index) const;
  ::std::string* mutable_player_name(int index);
  void set_player_name(int index, const ::std::string& value);
  #if LANG_CXX11
  void set_player_name(int index, ::std::string&& value);
  #endif
  void set_player_name(int index, const char* value);
  void set_player_name(int index, const void* value, size_t size);
  ::std::string* add_player_name();
  void add_player_name(const ::std::string& value);
  #if LANG_CXX11
  void add_player_name(::std::string&& value);
  #endif
  void add_player_name(const char* value);
  void add_player_name(const void* value, size_t size);
  const ::google::protobuf::RepeatedPtrField< ::std::string>& player_name() const;
  ::google::protobuf::RepeatedPtrField< ::std::string>* mutable_player_name();

  // repeated int32 player_level = 8;
  int player_level_size() const;
  void clear_player_level();
  static const int kPlayerLevelFieldNumber = 8;
  ::google::protobuf::int32 player_level(int index) const;
  void set_player_level(int index, ::google::protobuf::int32 value);
  void add_player_level(::google::protobuf::int32 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
      player_level() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
      mutable_player_level();

  // repeated int32 player_bt = 9;
  int player_bt_size() const;
  void clear_player_bt();
  static const int kPlayerBtFieldNumber = 9;
  ::google::protobuf::int32 player_bt(int index) const;
  void set_player_bt(int index, ::google::protobuf::int32 value);
  void add_player_bt(::google::protobuf::int32 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
      player_bt() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
      mutable_player_bt();

  // repeated int32 player_num = 10;
  int player_num_size() const;
  void clear_player_num();
  static const int kPlayerNumFieldNumber = 10;
  ::google::protobuf::int32 player_num(int index) const;
  void set_player_num(int index, ::google::protobuf::int32 value);
  void add_player_num(::google::protobuf::int32 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
      player_num() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
      mutable_player_num();

  // repeated int32 player_total = 11;
  int player_total_size() const;
  void clear_player_total();
  static const int kPlayerTotalFieldNumber = 11;
  ::google::protobuf::int32 player_total(int index) const;
  void set_player_total(int index, ::google::protobuf::int32 value);
  void add_player_total(::google::protobuf::int32 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
      player_total() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
      mutable_player_total();

  // repeated uint64 player_time = 12;
  int player_time_size() const;
  void clear_player_time();
  static const int kPlayerTimeFieldNumber = 12;
  ::google::protobuf::uint64 player_time(int index) const;
  void set_player_time(int index, ::google::protobuf::uint64 value);
  void add_player_time(::google::protobuf::uint64 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::uint64 >&
      player_time() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::uint64 >*
      mutable_player_time();

  // repeated int32 player_first = 13;
  int player_first_size() const;
  void clear_player_first();
  static const int kPlayerFirstFieldNumber = 13;
  ::google::protobuf::int32 player_first(int index) const;
  void set_player_first(int index, ::google::protobuf::int32 value);
  void add_player_first(::google::protobuf::int32 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
      player_first() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
      mutable_player_first();

  // repeated int32 player_vip = 14;
  int player_vip_size() const;
  void clear_player_vip();
  static const int kPlayerVipFieldNumber = 14;
  ::google::protobuf::int32 player_vip(int index) const;
  void set_player_vip(int index, ::google::protobuf::int32 value);
  void add_player_vip(::google::protobuf::int32 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
      player_vip() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
      mutable_player_vip();

  // repeated int32 player_achieve = 15;
  int player_achieve_size() const;
  void clear_player_achieve();
  static const int kPlayerAchieveFieldNumber = 15;
  ::google::protobuf::int32 player_achieve(int index) const;
  void set_player_achieve(int index, ::google::protobuf::int32 value);
  void add_player_achieve(::google::protobuf::int32 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
      player_achieve() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
      mutable_player_achieve();

  // repeated int32 player_chenghao = 16;
  int player_chenghao_size() const;
  void clear_player_chenghao();
  static const int kPlayerChenghaoFieldNumber = 16;
  ::google::protobuf::int32 player_chenghao(int index) const;
  void set_player_chenghao(int index, ::google::protobuf::int32 value);
  void add_player_chenghao(::google::protobuf::int32 value);
  const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
      player_chenghao() const;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
      mutable_player_chenghao();

  // uint64 guid = 1;
  void clear_guid();
  static const int kGuidFieldNumber = 1;
  ::google::protobuf::uint64 guid() const;
  void set_guid(::google::protobuf::uint64 value);

  // int32 template_id = 2;
  void clear_template_id();
  static const int kTemplateIdFieldNumber = 2;
  ::google::protobuf::int32 template_id() const;
  void set_template_id(::google::protobuf::int32 value);

  // int32 amount = 3;
  void clear_amount();
  static const int kAmountFieldNumber = 3;
  ::google::protobuf::int32 amount() const;
  void set_amount(::google::protobuf::int32 value);

  // @@protoc_insertion_point(class_scope:dhc.treasure_list_t)
 private:

  ::google::protobuf::internal::InternalMetadataWithArena _internal_metadata_;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 > nalflag_;
  mutable int _nalflag_cached_byte_size_;
  ::google::protobuf::RepeatedField< ::google::protobuf::uint64 > player_guid_;
  mutable int _player_guid_cached_byte_size_;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 > player_template_;
  mutable int _player_template_cached_byte_size_;
  ::google::protobuf::RepeatedPtrField< ::std::string> player_name_;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 > player_level_;
  mutable int _player_level_cached_byte_size_;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 > player_bt_;
  mutable int _player_bt_cached_byte_size_;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 > player_num_;
  mutable int _player_num_cached_byte_size_;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 > player_total_;
  mutable int _player_total_cached_byte_size_;
  ::google::protobuf::RepeatedField< ::google::protobuf::uint64 > player_time_;
  mutable int _player_time_cached_byte_size_;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 > player_first_;
  mutable int _player_first_cached_byte_size_;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 > player_vip_;
  mutable int _player_vip_cached_byte_size_;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 > player_achieve_;
  mutable int _player_achieve_cached_byte_size_;
  ::google::protobuf::RepeatedField< ::google::protobuf::int32 > player_chenghao_;
  mutable int _player_chenghao_cached_byte_size_;
  ::google::protobuf::uint64 guid_;
  ::google::protobuf::int32 template_id_;
  ::google::protobuf::int32 amount_;
  mutable ::google::protobuf::internal::CachedSize _cached_size_;
  friend struct ::protobuf_treasure_5flist_2eeproto::TableStruct;
};
// ===================================================================


// ===================================================================

#ifdef __GNUC__
  #pragma GCC diagnostic push
  #pragma GCC diagnostic ignored "-Wstrict-aliasing"
#endif  // __GNUC__
// treasure_list_t

// uint64 guid = 1;
inline void treasure_list_t::clear_guid() {
  set_changed();
  guid_ = GOOGLE_ULONGLONG(0);
}
inline ::google::protobuf::uint64 treasure_list_t::guid() const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.guid)
  return guid_;
}
inline void treasure_list_t::set_guid(::google::protobuf::uint64 value) {
  set_changed();
  
  guid_ = value;
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.guid)
}

// int32 template_id = 2;
inline void treasure_list_t::clear_template_id() {
  set_changed();
  template_id_ = 0;
}
inline ::google::protobuf::int32 treasure_list_t::template_id() const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.template_id)
  return template_id_;
}
inline void treasure_list_t::set_template_id(::google::protobuf::int32 value) {
  set_changed();
  
  template_id_ = value;
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.template_id)
}

// int32 amount = 3;
inline void treasure_list_t::clear_amount() {
  set_changed();
  amount_ = 0;
}
inline ::google::protobuf::int32 treasure_list_t::amount() const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.amount)
  return amount_;
}
inline void treasure_list_t::set_amount(::google::protobuf::int32 value) {
  set_changed();
  
  amount_ = value;
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.amount)
}

// repeated int32 nalflag = 4;
inline int treasure_list_t::nalflag_size() const {
  return nalflag_.size();
}
inline void treasure_list_t::clear_nalflag() {
  set_changed();
  nalflag_.Clear();
}
inline ::google::protobuf::int32 treasure_list_t::nalflag(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.nalflag)
  return nalflag_.Get(index);
}
inline void treasure_list_t::set_nalflag(int index, ::google::protobuf::int32 value) {
  set_changed();
  nalflag_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.nalflag)
}
inline void treasure_list_t::add_nalflag(::google::protobuf::int32 value) {
  set_changed();
  nalflag_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.nalflag)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
treasure_list_t::nalflag() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.nalflag)
  return nalflag_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
treasure_list_t::mutable_nalflag() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.nalflag)
  return &nalflag_;
}

// repeated uint64 player_guid = 5;
inline int treasure_list_t::player_guid_size() const {
  return player_guid_.size();
}
inline void treasure_list_t::clear_player_guid() {
  set_changed();
  player_guid_.Clear();
}
inline ::google::protobuf::uint64 treasure_list_t::player_guid(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_guid)
  return player_guid_.Get(index);
}
inline void treasure_list_t::set_player_guid(int index, ::google::protobuf::uint64 value) {
  set_changed();
  player_guid_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_guid)
}
inline void treasure_list_t::add_player_guid(::google::protobuf::uint64 value) {
  set_changed();
  player_guid_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_guid)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::uint64 >&
treasure_list_t::player_guid() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_guid)
  return player_guid_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::uint64 >*
treasure_list_t::mutable_player_guid() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_guid)
  return &player_guid_;
}

// repeated int32 player_template = 6;
inline int treasure_list_t::player_template_size() const {
  return player_template_.size();
}
inline void treasure_list_t::clear_player_template() {
  set_changed();
  player_template_.Clear();
}
inline ::google::protobuf::int32 treasure_list_t::player_template(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_template)
  return player_template_.Get(index);
}
inline void treasure_list_t::set_player_template(int index, ::google::protobuf::int32 value) {
  set_changed();
  player_template_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_template)
}
inline void treasure_list_t::add_player_template(::google::protobuf::int32 value) {
  set_changed();
  player_template_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_template)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
treasure_list_t::player_template() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_template)
  return player_template_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
treasure_list_t::mutable_player_template() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_template)
  return &player_template_;
}

// repeated bytes player_name = 7;
inline int treasure_list_t::player_name_size() const {
  return player_name_.size();
}
inline void treasure_list_t::clear_player_name() {
  set_changed();
  player_name_.Clear();
}
inline const ::std::string& treasure_list_t::player_name(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_name)
  return player_name_.Get(index);
}
inline ::std::string* treasure_list_t::mutable_player_name(int index) {
  set_changed();
  // @@protoc_insertion_point(field_mutable:dhc.treasure_list_t.player_name)
  return player_name_.Mutable(index);
}
inline void treasure_list_t::set_player_name(int index, const ::std::string& value) {
  set_changed();
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_name)
  player_name_.Mutable(index)->assign(value);
}
#if LANG_CXX11
inline void treasure_list_t::set_player_name(int index, ::std::string&& value) {
  set_changed();
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_name)
  player_name_.Mutable(index)->assign(std::move(value));
}
#endif
inline void treasure_list_t::set_player_name(int index, const char* value) {
  set_changed();
  GOOGLE_DCHECK(value != NULL);
  player_name_.Mutable(index)->assign(value);
  // @@protoc_insertion_point(field_set_char:dhc.treasure_list_t.player_name)
}
inline void treasure_list_t::set_player_name(int index, const void* value, size_t size) {
  set_changed();
  player_name_.Mutable(index)->assign(
    reinterpret_cast<const char*>(value), size);
  // @@protoc_insertion_point(field_set_pointer:dhc.treasure_list_t.player_name)
}
inline ::std::string* treasure_list_t::add_player_name() {
  set_changed();
  // @@protoc_insertion_point(field_add_mutable:dhc.treasure_list_t.player_name)
  return player_name_.Add();
}
inline void treasure_list_t::add_player_name(const ::std::string& value) {
  set_changed();
  player_name_.Add()->assign(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_name)
}
#if LANG_CXX11
inline void treasure_list_t::add_player_name(::std::string&& value) {
  set_changed();
  player_name_.Add(std::move(value));
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_name)
}
#endif
inline void treasure_list_t::add_player_name(const char* value) {
  set_changed();
  GOOGLE_DCHECK(value != NULL);
  player_name_.Add()->assign(value);
  // @@protoc_insertion_point(field_add_char:dhc.treasure_list_t.player_name)
}
inline void treasure_list_t::add_player_name(const void* value, size_t size) {
  set_changed();
  player_name_.Add()->assign(reinterpret_cast<const char*>(value), size);
  // @@protoc_insertion_point(field_add_pointer:dhc.treasure_list_t.player_name)
}
inline const ::google::protobuf::RepeatedPtrField< ::std::string>&
treasure_list_t::player_name() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_name)
  return player_name_;
}
inline ::google::protobuf::RepeatedPtrField< ::std::string>*
treasure_list_t::mutable_player_name() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_name)
  return &player_name_;
}

// repeated int32 player_level = 8;
inline int treasure_list_t::player_level_size() const {
  return player_level_.size();
}
inline void treasure_list_t::clear_player_level() {
  set_changed();
  player_level_.Clear();
}
inline ::google::protobuf::int32 treasure_list_t::player_level(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_level)
  return player_level_.Get(index);
}
inline void treasure_list_t::set_player_level(int index, ::google::protobuf::int32 value) {
  set_changed();
  player_level_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_level)
}
inline void treasure_list_t::add_player_level(::google::protobuf::int32 value) {
  set_changed();
  player_level_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_level)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
treasure_list_t::player_level() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_level)
  return player_level_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
treasure_list_t::mutable_player_level() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_level)
  return &player_level_;
}

// repeated int32 player_bt = 9;
inline int treasure_list_t::player_bt_size() const {
  return player_bt_.size();
}
inline void treasure_list_t::clear_player_bt() {
  set_changed();
  player_bt_.Clear();
}
inline ::google::protobuf::int32 treasure_list_t::player_bt(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_bt)
  return player_bt_.Get(index);
}
inline void treasure_list_t::set_player_bt(int index, ::google::protobuf::int32 value) {
  set_changed();
  player_bt_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_bt)
}
inline void treasure_list_t::add_player_bt(::google::protobuf::int32 value) {
  set_changed();
  player_bt_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_bt)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
treasure_list_t::player_bt() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_bt)
  return player_bt_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
treasure_list_t::mutable_player_bt() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_bt)
  return &player_bt_;
}

// repeated int32 player_num = 10;
inline int treasure_list_t::player_num_size() const {
  return player_num_.size();
}
inline void treasure_list_t::clear_player_num() {
  set_changed();
  player_num_.Clear();
}
inline ::google::protobuf::int32 treasure_list_t::player_num(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_num)
  return player_num_.Get(index);
}
inline void treasure_list_t::set_player_num(int index, ::google::protobuf::int32 value) {
  set_changed();
  player_num_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_num)
}
inline void treasure_list_t::add_player_num(::google::protobuf::int32 value) {
  set_changed();
  player_num_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_num)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
treasure_list_t::player_num() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_num)
  return player_num_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
treasure_list_t::mutable_player_num() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_num)
  return &player_num_;
}

// repeated int32 player_total = 11;
inline int treasure_list_t::player_total_size() const {
  return player_total_.size();
}
inline void treasure_list_t::clear_player_total() {
  set_changed();
  player_total_.Clear();
}
inline ::google::protobuf::int32 treasure_list_t::player_total(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_total)
  return player_total_.Get(index);
}
inline void treasure_list_t::set_player_total(int index, ::google::protobuf::int32 value) {
  set_changed();
  player_total_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_total)
}
inline void treasure_list_t::add_player_total(::google::protobuf::int32 value) {
  set_changed();
  player_total_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_total)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
treasure_list_t::player_total() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_total)
  return player_total_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
treasure_list_t::mutable_player_total() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_total)
  return &player_total_;
}

// repeated uint64 player_time = 12;
inline int treasure_list_t::player_time_size() const {
  return player_time_.size();
}
inline void treasure_list_t::clear_player_time() {
  set_changed();
  player_time_.Clear();
}
inline ::google::protobuf::uint64 treasure_list_t::player_time(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_time)
  return player_time_.Get(index);
}
inline void treasure_list_t::set_player_time(int index, ::google::protobuf::uint64 value) {
  set_changed();
  player_time_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_time)
}
inline void treasure_list_t::add_player_time(::google::protobuf::uint64 value) {
  set_changed();
  player_time_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_time)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::uint64 >&
treasure_list_t::player_time() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_time)
  return player_time_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::uint64 >*
treasure_list_t::mutable_player_time() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_time)
  return &player_time_;
}

// repeated int32 player_first = 13;
inline int treasure_list_t::player_first_size() const {
  return player_first_.size();
}
inline void treasure_list_t::clear_player_first() {
  set_changed();
  player_first_.Clear();
}
inline ::google::protobuf::int32 treasure_list_t::player_first(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_first)
  return player_first_.Get(index);
}
inline void treasure_list_t::set_player_first(int index, ::google::protobuf::int32 value) {
  set_changed();
  player_first_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_first)
}
inline void treasure_list_t::add_player_first(::google::protobuf::int32 value) {
  set_changed();
  player_first_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_first)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
treasure_list_t::player_first() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_first)
  return player_first_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
treasure_list_t::mutable_player_first() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_first)
  return &player_first_;
}

// repeated int32 player_vip = 14;
inline int treasure_list_t::player_vip_size() const {
  return player_vip_.size();
}
inline void treasure_list_t::clear_player_vip() {
  set_changed();
  player_vip_.Clear();
}
inline ::google::protobuf::int32 treasure_list_t::player_vip(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_vip)
  return player_vip_.Get(index);
}
inline void treasure_list_t::set_player_vip(int index, ::google::protobuf::int32 value) {
  set_changed();
  player_vip_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_vip)
}
inline void treasure_list_t::add_player_vip(::google::protobuf::int32 value) {
  set_changed();
  player_vip_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_vip)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
treasure_list_t::player_vip() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_vip)
  return player_vip_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
treasure_list_t::mutable_player_vip() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_vip)
  return &player_vip_;
}

// repeated int32 player_achieve = 15;
inline int treasure_list_t::player_achieve_size() const {
  return player_achieve_.size();
}
inline void treasure_list_t::clear_player_achieve() {
  set_changed();
  player_achieve_.Clear();
}
inline ::google::protobuf::int32 treasure_list_t::player_achieve(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_achieve)
  return player_achieve_.Get(index);
}
inline void treasure_list_t::set_player_achieve(int index, ::google::protobuf::int32 value) {
  set_changed();
  player_achieve_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_achieve)
}
inline void treasure_list_t::add_player_achieve(::google::protobuf::int32 value) {
  set_changed();
  player_achieve_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_achieve)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
treasure_list_t::player_achieve() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_achieve)
  return player_achieve_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
treasure_list_t::mutable_player_achieve() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_achieve)
  return &player_achieve_;
}

// repeated int32 player_chenghao = 16;
inline int treasure_list_t::player_chenghao_size() const {
  return player_chenghao_.size();
}
inline void treasure_list_t::clear_player_chenghao() {
  set_changed();
  player_chenghao_.Clear();
}
inline ::google::protobuf::int32 treasure_list_t::player_chenghao(int index) const {
  // @@protoc_insertion_point(field_get:dhc.treasure_list_t.player_chenghao)
  return player_chenghao_.Get(index);
}
inline void treasure_list_t::set_player_chenghao(int index, ::google::protobuf::int32 value) {
  set_changed();
  player_chenghao_.Set(index, value);
  // @@protoc_insertion_point(field_set:dhc.treasure_list_t.player_chenghao)
}
inline void treasure_list_t::add_player_chenghao(::google::protobuf::int32 value) {
  set_changed();
  player_chenghao_.Add(value);
  // @@protoc_insertion_point(field_add:dhc.treasure_list_t.player_chenghao)
}
inline const ::google::protobuf::RepeatedField< ::google::protobuf::int32 >&
treasure_list_t::player_chenghao() const {
  // @@protoc_insertion_point(field_list:dhc.treasure_list_t.player_chenghao)
  return player_chenghao_;
}
inline ::google::protobuf::RepeatedField< ::google::protobuf::int32 >*
treasure_list_t::mutable_player_chenghao() {
  set_changed();
  // @@protoc_insertion_point(field_mutable_list:dhc.treasure_list_t.player_chenghao)
  return &player_chenghao_;
}

#ifdef __GNUC__
  #pragma GCC diagnostic pop
#endif  // __GNUC__

// @@protoc_insertion_point(namespace_scope)

}  // namespace dhc

// @@protoc_insertion_point(global_scope)

#endif  // PROTOBUF_INCLUDED_treasure_5flist_2eeproto

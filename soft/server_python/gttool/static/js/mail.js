$(document).ready(function () {
    $("#id_reward1_1").change(function () {
        $("#id_reward1_2").empty();
        var v = $("#id_reward1_1").val();
        if (v == 0) {
            return;
        }
        else if (v == 1) {
            set_ziyuan("#id_reward1_2");
        }
        else if (v == 2) {
            set_item("#id_reward1_2");
        }
        else if (v == 4) {
            set_equip("#id_reward1_2");
        }
        else if (v == 3) {
            set_role("#id_reward1_2");
        }
        else if (v == 6) {
            set_baowu("#id_reward1_2");
        }
        else if (v == 500) {
            $("#id_reward1_2").append($("<option>").val(1).text("无"));
        }
        else if (v == 600) {
            $("#id_reward1_2").append($("<option>").val(1).text("无"));
        }
        else {
            set_sort("#id_reward1_2");
        }
    });

    $("#id_reward2_1").change(function () {
        $("#id_reward2_2").empty();
        var v = $("#id_reward2_1").val();
        if (v == 0) {
            return;
        }
        else if (v == 1) {
            set_ziyuan("#id_reward2_2");
        }
        else if (v == 2) {
            set_item("#id_reward2_2");
        }
        else if (v == 4) {
            set_equip("#id_reward2_2");
        }
        else if (v == 6) {
            set_baowu("#id_reward2_2");
        }
        else {
            set_sort("#id_reward2_2");
        }
    });

    $("#id_reward3_1").change(function () {
        $("#id_reward3_2").empty();
        var v = $("#id_reward3_1").val();
        if (v == 0) {
            return;
        }
        else if (v == 1) {
            set_ziyuan("#id_reward3_2");
        }
        else if (v == 2) {
            set_item("#id_reward3_2");
        }
        else if (v == 4) {
            set_equip("#id_reward3_2");
        }
        else if (v == 6) {
            set_baowu("#id_reward3_2");
        }
        else {
            set_sort("#id_reward3_2");
        }
    });

    $("#id_reward4_1").change(function () {
        $("#id_reward4_2").empty();
        var v = $("#id_reward4_1").val();
        if (v == 0) {
            return;
        }
        else if (v == 1) {
            set_ziyuan("#id_reward4_2");
        }
        else if (v == 2) {
            set_item("#id_reward4_2");
        }
        else if (v == 4) {
            set_equip("#id_reward4_2");
        }
        else if (v == 6) {
            set_baowu("#id_reward4_2");
        }
        else {
            set_sort("#id_reward4_2");
        }
    });

    $("#id_duihuan1_1").change(function () {
        $("#id_duihuan1_2").empty();
        var v = $("#id_duihuan1_1").val();
        if (v == 0) {
            return;
        }
        else if (v == 1) {
            set_ziyuan("#id_duihuan1_2");
        }
        else if (v == 2) {
            set_item("#id_duihuan1_2");
        }
        else if (v == 4) {
            set_equip("#id_duihuan1_2");
        }
        else if (v == 6) {
            set_baowu("#id_duihuan1_2");
        }
        else {
            set_sort("#id_duihuan1_2");
        }
    });

    $("#id_duihuan2_1").change(function () {
        $("#id_duihuan2_2").empty();
        var v = $("#id_duihuan2_1").val();
        if (v == 0) {
            return;
        }
        else if (v == 1) {
            set_ziyuan("#id_duihuan2_2");
        }
        else if (v == 2) {
            set_item("#id_duihuan2_2");
        }
        else if (v == 4) {
            set_equip("#id_duihuan2_2");
        }
        else if (v == 6) {
            set_baowu("#id_duihuan2_2");
        }
        else {
            set_sort("#id_duihuan2_2");
        }
    });
});

function set_ziyuan(name)
{
    var option = $("<option>").val(1).text("金币");
    $(name).append(option);
    option = $("<option>").val(2).text("钻石");
    $(name).append(option);
    option = $("<option>").val(4).text("经验");
    $(name).append(option);
    option = $("<option>").val(5).text("战魂");
    $(name).append(option);
    option = $("<option>").val(6).text("合金");
    $(name).append(option);
    option = $("<option>").val(7).text("原力");
	$(name).append(option);
    option = $("<option>").val(8).text("日常积分");
    $(name).append(option);
    option = $("<option>").val(9).text("VIP经验");
    $(name).append(option);
    option = $("<option>").val(10).text("军团贡献");
    $(name).append(option);
	option = $("<option>").val(13).text("魔王勋章");
    $(name).append(option);
	option = $("<option>").val(14).text("荣誉点");
    $(name).append(option);
	option = $("<option>").val(15).text("能量");
	$(name).append(option);
	option = $("<option>").val(16).text("军团荣誉");
	$(name).append(option);
	option = $("<option>").val(18).text("魔王讨伐次数");
	$(name).append(option);
	option = $("<option>").val(19).text("时装图纸");
	$(name).append(option);
	option = $("<option>").val(20).text("幸运点数");
	$(name).append(option);
	option = $("<option>").val(21).text("回忆结晶");
	$(name).append(option);
	option = $("<option>").val(22).text("猎人勋章");
	$(name).append(option);
	option = $("<option>").val(24).text("冰晶");
	$(name).append(option);
	option = $("<option>").val(27).text("芯片");
	$(name).append(option);
}

function read_txt(name)
{
	var sorts = [];
    htmlobj = $.ajax({ url: "/static/other/" + name + ".txt?15", async: false });
    var text = htmlobj.responseText;
    var x = 0;
    var y = 0;
    var index1 = 0;
    var index2 = 0;
    var s;
	var txts = [];
	var txt = [];
    for (var i = 0; i < text.length; i++) {
        if (text.charAt(i) == '\r' || text.charAt(i) == '\t') {
            index1 = index2;
            index2 = i + 1;
            if (y > 1) {
                s = text.substring(index1, index2 - 1);
				txt.push(s);
            }
        }
        if (text.charAt(i) == '\n') {
            x = 0;
			y++;
			index1 = i + 1;
			index2 = i + 1;
			if (txt.length > 0)
			{
				txts.push(txt);
				txt = [];
			}
        }
        if (text.charAt(i) == '\t') {
            x++;
        }
    }
	return txts;
}

function add_sort(items)
{
    var sorts = read_txt("t_sort");
	
	var new_items = [];
	for (var i = 0; i < sorts.length; i++) {
		for (var j = 0; j < items.length; j++) {
			if (items[j][0] == sorts[i][0])
			{
				new_items.push(items[j]);
				break;
			}
		}
	}
	new_items = new_items.concat(items)
    return new_items;
}

function set_item(name)
{
	var items = read_txt("t_item");
	
    items = add_sort(items);

    for (var i = 0; i < items.length; i++) {
        s1 = items[i][0];
        s2 = s1 + "." + items[i][1];
        var option = $("<option>").val(parseInt(s1)).text(s2);
        $(name).append(option);
    }
}

function set_baowu(name)
{
	var baowus = read_txt("t_baowu");
	
    for (var i = 0; i < baowus.length; i++) {
        s1 = baowus[i][0];
        s2 = s1 + "." + baowus[i][1];
        var option = $("<option>").val(parseInt(s1)).text(s2);
        $(name).append(option);
    }
}

function set_equip(name)
{
	var equips = read_txt("t_equip");
	
    for (var i = 0; i < equips.length; i++) {
        s1 = equips[i][0];
        s2 = s1 + "." + equips[i][1];
        var option = $("<option>").val(parseInt(s1)).text(s2);
        $(name).append(option);
    }
}


function set_role(name) {
	var roles = read_txt("t_role");
	
    for (var i = 0; i < roles.length; i++) {
        s1 = roles[i][0];
        s2 = s1 + "." + roles[i][1];
        var option = $("<option>").val(parseInt(s1)).text(s2);
        $(name).append(option);
    }
}

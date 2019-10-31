$(document).ready(function () {
    var es = document.getElementsByName("libao_reward");
    for (var i = 0; i < es.length; ++i) {
        var e = es[i];
        var s = e.innerHTML;
        var sss = "";
        var words = s.split(' ');
        var num = parseInt(words.length / 4);
        for (var j = 0; j < num; ++j) {
            var ss = "";
            if (words[j * 4] == "1")
            {
                if (words[j * 4 + 1] == "1") {
                    ss = "金币*" + words[j * 4 + 2];
                }
                else if (words[j * 4 + 1] == "2") {
                    ss = "钻石*" + words[j * 4 + 2];
                }
                else if (words[j * 4 + 1] == "3") {
                    ss = "体力*" + words[j * 4 + 2];
                }
                else if (words[j * 4 + 1] == "4") {
                    ss = "经验*" + words[j * 4 + 2];
                }
                else if (words[j * 4 + 1] == "5") {
                    ss = "战魂*" + words[j * 4 + 2];
                }
                else if (words[j * 4 + 1] == "6") {
                    ss = "合金*" + words[j * 4 + 2];
                }
                else if (words[j * 4 + 1] == "7") {
                    ss = "原力*" + words[j * 4 + 2];
                }
                else if (words[j * 4 + 1] == "8") {
                    ss = "日常积分*" + words[j * 4 + 2];
                }
				else if (words[j * 4 + 1] == "9") {
                    ss = "VIP经验*" + words[j * 4 + 2];
                }
                else if (words[j * 4 + 1] == "10") {
                    ss = "军团贡献*" + words[j * 4 + 2];
                }
                else if (words[j * 4 + 1] == "11") {
                    ss = "宝石原石*" + words[j * 4 + 2];
                }
				else if (words[j * 4 + 1] == "12") {
                    ss = "探索次数*" + words[j * 4 + 2];
                }
				else if (words[j * 4 + 1] == "13") {
                    ss = "魔王勋章*" + words[j * 4 + 2];
                }
				else if (words[j * 4 + 1] == "14") {
                    ss = "竞技点*" + words[j * 4 + 2];
                }
				else if (words[j * 4 + 1] == "15") {
                    ss = "能量*" + words[j * 4 + 2];
				}
				else if (words[j * 4 + 1] == "16") {
				    ss = "军团荣誉*" + words[j * 4 + 2];
				}
				else if (words[j * 4 + 1] == "18") {
				    ss = "魔王讨伐次数*" + words[j * 4 + 2];
				}
				else if (words[j * 4 + 1] == "19") {
				    ss = "时装图纸*" + words[j * 4 + 2];
				}
				else if (words[j * 4 + 1] == "20") {
				    ss = "幸运点数*" + words[j * 4 + 2];
				}
				else if (words[j * 4 + 1] == "21") {
				    ss = "回忆结晶*" + words[j * 4 + 2];
				}
				else if (words[j * 4 + 1] == "22") {
				    ss = "猎人勋章*" + words[j * 4 + 2];
				}
				else if (words[j * 4 + 1] == "24") {
				    ss = "冰晶*" + words[j * 4 + 2];
				}
				else if (words[j * 4 + 1] == "27") {
				    ss = "芯片*" + words[j * 4 + 2];
				}
            }
            else if (words[j * 4] == "2")
            {
                ss = get_item(words[j * 4 + 1]) + "*" + words[j * 4 + 2];
            }
            else if (words[j * 4] == "4")
            {
                ss = get_equip(words[j * 4 + 1]);
            }
            else if (words[j * 4] == "6")
            {
                ss = get_baowu(words[j * 4 + 1]);
            }
            else if (words[j * 4] == "3")
            {
                ss = get_role(words[j * 4 + 1])
            }
            else if (words[j * 4] == "100") {
                ss = "充值ID:" + words[j * 4 + 1];
            }
            sss += "[" + ss + "]";
        }
        e.innerHTML = sss;
    }
});

function get_item(id) {
    htmlobj = $.ajax({ url: "/static/other/t_item.txt?11", async: false });
    var text = htmlobj.responseText;
    var x = 0;
    var y = 0;
    var index1 = 0;
    var index2 = 0;
    var s1;
    var s2;
    var flag = false;
    for (var i = 0; i < text.length; i++) {
        if (text.charAt(i) == '\n') {
            y++;
        }
        if (text.charAt(i) == '\n' || text.charAt(i) == '\t') {
            index1 = index2;
            index2 = i + 1;
            if (y > 1) {
                if (x == 0) {
                    s1 = text.substring(index1, index2 - 1);
                    if (s1 == id)
                    {
                        flag = true;
                    }
                }
                else if (x == 1 && flag) {
                    s2 = text.substring(index1, index2 - 1);
                    return s2;
                }
            }
        }
        if (text.charAt(i) == '\n') {
            x = 0;
        }
        if (text.charAt(i) == '\t') {
            x++;
        }
    }
    return "";
}

function get_baowu(id)
{
    htmlobj = $.ajax({ url: "/static/other/t_baowu.txt?11", async: false });
    var text = htmlobj.responseText;
    var x = 0;
    var y = 0;
    var index1 = 0;
    var index2 = 0;
    var s1;
    var s2;
    var flag = false;
    for (var i = 0; i < text.length; i++) {
        if (text.charAt(i) == '\n') {
            y++;
        }
        if (text.charAt(i) == '\n' || text.charAt(i) == '\t') {
            index1 = index2;
            index2 = i + 1;
            if (y > 1) {
                if (x == 0) {
                    s1 = text.substring(index1, index2 - 1);
                    if (s1 == id)
                    {
                        flag = true;
                    }
                }
                else if (x == 1 && flag) {
                    s2 = text.substring(index1, index2 - 1);
                    return s2;
                }
            }
        }
        if (text.charAt(i) == '\n') {
            x = 0;
        }
        if (text.charAt(i) == '\t') {
            x++;
        }
    }
    return "";
}

function get_equip(id)
{
    htmlobj = $.ajax({ url: "/static/other/t_equip.txt?11", async: false });
    var text = htmlobj.responseText;
    var x = 0;
    var y = 0;
    var index1 = 0;
    var index2 = 0;
    var s1;
    var s2;
    var flag = false;
    for (var i = 0; i < text.length; i++) {
        if (text.charAt(i) == '\n') {
            y++;
        }
        if (text.charAt(i) == '\n' || text.charAt(i) == '\t') {
            index1 = index2;
            index2 = i + 1;
            if (y > 1) {
                if (x == 0) {
                    s1 = text.substring(index1, index2 - 1);
                    if (s1 == id)
                    {
                        flag = true;
                    }
                }
                else if (x == 1 && flag) {
                    s2 = text.substring(index1, index2 - 1);
                    return s2;
                }
            }
        }
        if (text.charAt(i) == '\n') {
            x = 0;
        }
        if (text.charAt(i) == '\t') {
            x++;
        }
    }
    return "";
}

function get_role(id) {
    htmlobj = $.ajax({ url: "/static/other/t_role.txt?11", async: false });
    var text = htmlobj.responseText;
    var x = 0;
    var y = 0;
    var index1 = 0;
    var index2 = 0;
    var s1;
    var s2;
    var flag = false;
    for (var i = 0; i < text.length; i++) {
        if (text.charAt(i) == '\n') {
            y++;
        }
        if (text.charAt(i) == '\n' || text.charAt(i) == '\t') {
            index1 = index2;
            index2 = i + 1;
            if (y > 1) {
                if (x == 0) {
                    s1 = text.substring(index1, index2 - 1);
                    if (s1 == id) {
                        flag = true;
                    }
                }
                else if (x == 3 && flag) {
                    s2 = text.substring(index1, index2 - 1);
                    return s2;
                }
            }
        }
        if (text.charAt(i) == '\n') {
            x = 0;
        }
        if (text.charAt(i) == '\t') {
            x++;
        }
    }
    return "";
}

var canCreatCurrent = true;
var listStandarWidth = 0;//程序启动时计算标准宽度
var listShowedCount = 3;
var sidebarWidth = 200;//加右侧边框200
var sidebarMinWidth = 40;

function Move(itemId, oldIndex, newIndex, oldListID)
{
    jQuery('#activity_pane').showLoading();
    $.ajax({
        url: "UserStory/ChangeOrder",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ ItemId: itemId, OldIndex: oldIndex, NewIndex: newIndex, OldListID: oldListID, NewListID: "" }),
        success: function (response) {
            jQuery('#activity_pane').hideLoading();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("error!\r\n"+textStatus);
            jQuery('#activity_pane').hideLoading();
        }
    });
}
function MoveCrossList(itemId,oldIndex ,newIndex, oldListID ,newListID)
{
    jQuery('#activity_pane').showLoading();
    $.ajax({
        url: "UserStory/ChangeOrder",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ ItemId: itemId, OldIndex: oldIndex, NewIndex: newIndex, OldListID: oldListID, NewListID: newListID }),
        success: function (response) {
            jQuery('#activity_pane').hideLoading();
        }
    });
}

function ChangeStatus(itemId, result, scode) {
    jQuery('#activity_pane').showLoading();
    $.ajax({
        url: "UserStory/ChangeStatus",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ ItemID: itemId, ApprovalResult: result }),
        success: function (response) {
            var buttons = $("a[BtnType='StatusBtn'][itemid='" + itemId + "']")[0];
            var p = buttons.parentNode;
            $(p).html(response);
            $("a[BtnType='StatusBtn'][itemid='" + itemId + "']").click(function () {
                    var itemID = $(this).attr("itemid");
                    //alert(itemID);
                    ChangeStatus(itemID);
                    jQuery('#activity_pane').hideLoading();
                    return false;
            });
        }
    });
}

$(function () {

    $("[name='changeItemStatus']").click(function () {
        var itemID = $(this).attr("itemid");
        //alert(itemID);
        ChangeStatus(itemID);
        return false;
    });

    $("span[name='btn_addStory']").click(function () {
        if (canCreatCurrent)
        {
            jQuery('#activity_pane').showLoading();
            //get ul name
            var ulID = $(this).attr("listid");
            var ul = $('#' + ulID);
            var li = ul.prepend("<li id='storyAddArea' class='block_content_li_expand'></li>");
            $("#storyAddArea").load("/userStory/create?listId="+ulID);
            canCreatCurrent = false;
            jQuery('#activity_pane').hideLoading();
        }
    });
    
    $("span[name='btn_edit']").click(function () {
        var current_li = $(this).parent();
        var itemID = current_li.attr("itemid");
        var placeHolderID = "currentEditArea_" + itemID;
        var spanID = "btn_edit_" + itemID;
        var current_span = $("#" + spanID);
        if(current_span.hasClass("glyphicon-triangle-right"))
        {
            jQuery('#activity_pane').showLoading();
            //要展开
            ExpandStoryItem(current_li, current_span, placeHolderID, itemID);
            jQuery('#activity_pane').hideLoading();
        }
        else
        {
            CloseStroyItem(current_span, placeHolderID);
        }
    });

    $("#m_btn_current").click(function () {
        showHideList("block_current");
    });
    $("#m_btn_backlog").click(function () {
        showHideList("block_backlog");
    });
    $("#m_btn_icebox").click(function () {
        showHideList("block_icebox");
    });
    $("#m_btn_done").click(function () {
        showHideList("block_done");
    });
    $("#m_btn_showhidemenu").click(function () {
        var sideBar = $("#mendu_side_bar");
        if (sideBar.width() == sidebarMinWidth)
        {
            //show
            AdjustPlace(sidebarWidth);
            $("#project_title").show();
            $("#m_btn_current").text("Current");
            $("#m_btn_backlog").text("Backlog");
            $("#m_btn_icebox").text("ICEBox");
            $("#m_btn_done").text("Done");
        }
        else
        {
            //hide
            AdjustPlace(sidebarMinWidth);
            $("#project_title").hide();
            $("#m_btn_current").text("C");
            $("#m_btn_backlog").text("B");
            $("#m_btn_icebox").text("I");
            $("#m_btn_done").text("D");
        }
    });

    AdjustPlace(sidebarWidth);
});

function AdjustPlace(leftWidth)
{
    var space = 5;//间隙宽度

    var w = $(window).innerWidth();
    var container_width = w - leftWidth-1;//左侧右边有一条竖线
    var cw = (w - leftWidth - space * 3 - 1 * 6) / 3;
    var columWidth = parseInt(cw)-1;

    $("#story_container").width(container_width);

    listStandarWidth = columWidth;
    $("#mendu_side_bar").width(leftWidth);
    

    $("#block_current").width(columWidth);
    $("#block_backlog").width(columWidth);
    $("#block_icebox").width(columWidth);
    $("#block_done").width(columWidth);
}

function showHideList(listID)
{
    var listItem = $("#" + listID);
    if (listItem.css("display") == "none") {
        listItem.show();
        listItem.width(listStandarWidth);
        listShowedCount += 1;
        if (listShowedCount > 3)
        {
            $("#story_container").css("overflow-x", "scroll");
        }
            

    }
    else {
        listItem.hide();
        listItem.width(0);
        listShowedCount -= 1;

        if (listShowedCount <= 3)
        {
            $("#story_container").css("overflow-x", "hidden");
        }
            
    }
}


function ExpandStoryItem(liItem, spanItem, placeHolderID,itemID)
{
    var li = liItem.after("<li id='" + placeHolderID + "' class='block_content_li_expand'></li>");
    $(spanItem).removeClass("glyphicon-triangle-right");
    $(spanItem).addClass("glyphicon-triangle-bottom");
    $("#" + placeHolderID).load("/userStory/edit?listId=Current&itemId=" + itemID);
}
function CloseStroyItem(spanItem, placeHolderID)
{
    $(spanItem).removeClass("glyphicon-triangle-bottom");
    $(spanItem).addClass("glyphicon-triangle-right");
    $("#" + placeHolderID).remove();
}







(function () {
	'use strict';
	var byId = function (id) { return document.getElementById(id); },
		loadScripts = function (desc, callback) {
			var deps = [], key, idx = 0;

			for (key in desc) {
				deps.push(key);
			}

			(function _next() {
				var pid,
					name = deps[idx],
					script = document.createElement('script');

				script.type = 'text/javascript';
				script.src = desc[deps[idx]];

				pid = setInterval(function () {
					if (window[name]) {
						clearTimeout(pid);

						deps[idx++] = window[name];

						if (deps[idx]) {
							_next();
						} else {
							callback.apply(null, deps);
						}
					}
				}, 30);

				document.getElementsByTagName('head')[0].appendChild(script);
			})()
		},

		console = window.console;


	if (!console.log) {
		console.log = function () {
			alert([].join.apply(arguments, ' '));
		};
	}

	Sortable.create(byId('Backlog'), {
	    group: "mygroup",
	    animation: 150,
	    onAdd: function (evt) {
	        console.log('onAdd.todo:', [evt.item, evt.from]);
	        MoveCrossList($(evt.item).attr("itemid"),evt.oldIndex ,evt.newIndex, evt.from.id, evt.to.id);
	    },
	    onUpdate: function (evt) {
	        console.log('onUpdate.todo:', [evt.item, evt.from]);
	        Move($(evt.item).attr("itemid"), evt.oldIndex, evt.newIndex, "Backlog");
	    },
	    onRemove: function (evt) { console.log('onRemove.todo:', [evt.item, evt.from]); },
	    onStart: function (evt) { console.log('onStart.todo:', [evt.item, evt.from]); },
	    onSort: function (evt) { console.log('onStart.todo:', [evt.item, evt.from]); },
	    onEnd: function (evt) { console.log('onEnd.todo:', [evt.item, evt.from]); }
	});

	Sortable.create(byId('Current'), {
	    group: "mygroup",
		animation: 150,
		//store: {
		//	get: function (sortable) {
		//		var order = localStorage.getItem(sortable.options.group);
		//		return order ? order.split('|') : [];
		//	},
		//	set: function (sortable) {
		//		var order = sortable.toArray();
		//		localStorage.setItem(sortable.options.group, order.join('|'));
		//	}
		//},
		onAdd: function (evt) {
		    console.log('onAdd.current:', [evt.item, evt.from]);
		    MoveCrossList($(evt.item).attr("itemid"),evt.oldIndex, evt.newIndex, evt.from.id, evt.to.id);
		},
		onUpdate: function (evt) {
		    console.log('onUpdate.current:', [evt.item, evt.from]);
		    Move($(evt.item).attr("itemid"), evt.oldIndex, evt.newIndex, "Current");
		},
		onRemove: function (evt) {


		    console.log('onRemove.current:', [evt.item, evt.from]);


		},
		onStart: function (evt) { console.log('onStart.current:', [evt.item, evt.from]); },
		onSort: function (evt) {
		    console.log('onSort.current:', [evt.item, evt.from]);
		    //Move($(evt.item.children[0].children[0]).attr("itemid"), evt.oldIndex, evt.newIndex, "Current");
		},
		onEnd: function (evt) {
		    console.log('onEnd.current:', [evt.item, evt.from]);
		    //
		    //alert("我是第"+evt.item.index()+"个");
		}
	});

	Sortable.create(byId('Doing'), {
	    group: "mygroup",
		animation: 150,
		onAdd: function (evt) {
		    console.log('onAdd.doing:', evt.item);
		    //Move(evt.itemId, 3);
		},
		onUpdate: function (evt) { console.log('onUpdate.doing:', evt.item); },
		onRemove: function (evt) {
		    console.log('onRemove.doing:', evt.item);
		},
		onStart: function (evt) {
		    console.log('onStart.doing:', evt.item);
		},
		onEnd: function (evt) {
		    console.log('onEnd.doing:', evt.item);
		}
	});
	Sortable.create(byId('Done'), {
	    group: "mygroup",
	    animation: 150,
	    onAdd: function (evt) {
	        console.log('onAdd.done:', evt.item);
	        //Move(evt.itemId, 4);
	    },
	    onUpdate: function (evt) { console.log('onUpdate.done:', evt.item); },
	    onRemove: function (evt) {
	        console.log('onRemove.done:', evt.item);
	    },
	    onStart: function (evt) {
	        console.log('onStart.done:', evt.item);
	    },
	    onEnd: function (evt) {
	        console.log('onEnd.done:', evt.item);
	    }
	});


	//// Multi groups
	//Sortable.create(byId('multi'), {
	//	animation: 150,
	//	draggable: '.tile',
	//	handle: '.tile__name'
	//});

	//[].forEach.call(byId('multi').getElementsByClassName('tile__list'), function (el){
	//	Sortable.create(el, {
	//		group: 'photo',
	//		animation: 150
	//	});
	//});


	//// Editable list
	//var editableList = Sortable.create(byId('editable'), {
	//	animation: 150,
	//	filter: '.js-remove',
	//	onFilter: function (evt) {
	//		evt.item.parentNode.removeChild(evt.item);
	//	}
	//});


	//byId('addUser').onclick = function () {
	//	Ply.dialog('prompt', {
	//		title: 'Add',
	//		form: { name: 'name' }
	//	}).done(function (ui) {
	//		var el = document.createElement('li');
	//		el.innerHTML = ui.data.name + '<i class="js-remove">✖</i>';
	//		editableList.el.appendChild(el);
	//	});
	//};


	//// Advanced groups
	//[{
	//	name: 'advanced',
	//	pull: true,
	//	put: true
	//},
	//{
	//	name: 'advanced',
	//	pull: 'clone',
	//	put: false
	//}, {
	//	name: 'advanced',
	//	pull: false,
	//	put: true
	//}].forEach(function (groupOpts, i) {
	//	Sortable.create(byId('advanced-' + (i + 1)), {
	//		sort: (i != 1),
	//		group: groupOpts,
	//		animation: 150
	//	});
	//});


	//// 'handle' option
	//Sortable.create(byId('handle-1'), {
	//	handle: '.drag-handle',
	//	animation: 150
    //});





})();


// Background
document.addEventListener("DOMContentLoaded", function () {
	function setNoiseBackground(el, width, height, opacity) {
		var canvas = document.createElement("canvas");
		var context = canvas.getContext("2d");

		canvas.width = width;
		canvas.height = height;

		for (var i = 0; i < width; i++) {
			for (var j = 0; j < height; j++) {
				var val = Math.floor(Math.random() * 255);
				context.fillStyle = "rgba(" + val + "," + val + "," + val + "," + opacity + ")";
				context.fillRect(i, j, 1, 1);
			}
		}

		el.style.background = "url(" + canvas.toDataURL("image/png") + ")";
	}

	setNoiseBackground(document.getElementsByTagName('body')[0], 50, 50, 0.02);
}, false);

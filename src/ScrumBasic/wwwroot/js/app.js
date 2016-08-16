function Move(itemId,oldIndex,newIndex,oldListID)
{
    $.ajax({
        url: "UserStory/ChangeOrder",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ ItemId: itemId, OldIndex: oldIndex, NewIndex: newIndex, OldListID: oldListID, NewListID: "" }),
        success: function (response) {
            response ? alert("It worked!")
            : alert("It didn't work.");
        }
    });
}
function MoveCrossList(itemId, newIndex, oldListID ,newListID)
{
    $.ajax({
        url: "UserStory/ChangeOrder",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ ItemId: itemId, OldIndex: -1, NewIndex: newIndex, OldListID: oldListID, NewListID: newListID }),
        success: function (response) {
            response ? alert("It worked!")
            : alert("It didn't work.");
        }
    });
}

function ChangeStatus(itemId,result,scode) {
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
                    return false;
            });
        }
    });
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
	        MoveCrossList($(evt.item.children[0].children[0]).attr("itemid"), evt.newIndex, evt.from.id, evt.to.id);
	    },
	    onUpdate: function (evt) {
	        console.log('onUpdate.todo:', [evt.item, evt.from]);
	        Move($(evt.item.children[0].children[0]).attr("itemid"), evt.oldIndex, evt.newIndex, "Backlog");
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
		    console.log('onAdd.backlog:', [evt.item, evt.from]);
		    MoveCrossList($(evt.item.children[0].children[0]).attr("itemid"), evt.newIndex, evt.from.id, evt.to.id);
		},
		onUpdate: function (evt) {
		    console.log('onUpdate.backlog:', [evt.item, evt.from]);
		    Move($(evt.item.children[0].children[0]).attr("itemid"), evt.oldIndex, evt.newIndex, "Backlog");
		},
		onRemove: function (evt) {


		    console.log('onRemove.backlog:', [evt.item, evt.from]);


		},
		onStart: function (evt) { console.log('onStart.backlog:', [evt.item, evt.from]); },
		onSort: function (evt) {
		    console.log('onSort.backlog:', [evt.item, evt.from]);
		    //Move($(evt.item.children[0].children[0]).attr("itemid"), evt.oldIndex, evt.newIndex, "Current");
		},
		onEnd: function (evt) {
		    console.log('onEnd.backlog:', [evt.item, evt.from]);
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

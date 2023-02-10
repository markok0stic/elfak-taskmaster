function hide(element){
    element.classList.remove("show");
    element.classList.add("hide");
}

function createComment(text) {
    var comment = document.createElement('p');
    comment.id = "comment";
    comment.innerHTML = `${text}`;
    document.getElementById('comments').appendChild(comment);
}

function show(element) {
    element.classList.remove("hide");
    element.classList.add("show");
}

var findBtns = document.querySelectorAll("#findBtn");
findBtns.forEach(btn => {
    btn.addEventListener("click", () => {
        hide(btn.closest(".centerPosition").children[1]);
        show(btn.closest(".centerPosition").children[2]);
    });
});

var addBtns = document.querySelectorAll("#addBtn");
addBtns.forEach(btn => {
    btn.addEventListener("click", () => {
        hide(btn.closest(".centerPosition").children[2]);
        show(btn.closest(".centerPosition").children[1]);
    });
});

document.getElementById('selectProject').addEventListener("click", () => {
    show(document.getElementById('sprint'));
})

document.getElementById('selectSprint').addEventListener("click", () => {
    show(document.getElementById('task'));
})

document.getElementById('selectTask').addEventListener("click", () => {
    show(document.getElementById('manageTask'));
})
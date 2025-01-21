function updateBoxPosition() {
    var register_box = document.getElementsByClassName('glass-container')[0];

    if (register_box) {
        var boxWidth = parseFloat(window.getComputedStyle(register_box).width);
        var boxHeight = parseFloat(window.getComputedStyle(register_box).height);

        register_box.style.position = 'absolute';
        register_box.style.top = (window.innerHeight / 2) - (boxHeight / 2) + 'px';
        register_box.style.left = (window.innerWidth / 2) - (boxWidth / 2) + 'px';
    } else {
        console.log("找不到 .glass-container 元素");
    }
}

updateBoxPosition();
window.onresize = updateBoxPosition;

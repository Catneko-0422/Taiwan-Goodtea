function updateBoxPosition() {
    var login_box = document.getElementsByClassName('glass-container')[0];

    if (login_box) {
        var boxWidth = parseFloat(window.getComputedStyle(login_box).width);
        var boxHeight = parseFloat(window.getComputedStyle(login_box).height);

        login_box.style.position = 'absolute';
        login_box.style.top = (window.innerHeight / 2) - (boxHeight / 2) + 'px';
        login_box.style.left = (window.innerWidth / 2) - (boxWidth / 2) + 'px';
    } else {
        console.log("找不到 .glass-container 元素");
    }
}

updateBoxPosition();
window.onresize = updateBoxPosition;

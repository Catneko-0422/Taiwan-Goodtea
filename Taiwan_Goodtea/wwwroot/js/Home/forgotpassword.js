document.addEventListener("DOMContentLoaded", function () {
    function updateBoxPosition() {
        var forgotpassword_box = document.getElementsByClassName('glass-container')[0];

        if (forgotpassword_box) {
            var boxWidth = parseFloat(window.getComputedStyle(forgotpassword_box).width);
            var boxHeight = parseFloat(window.getComputedStyle(forgotpassword_box).height);

            forgotpassword_box.style.position = 'absolute';
            forgotpassword_box.style.top = (window.innerHeight / 2) - (boxHeight / 2) + 'px';
            forgotpassword_box.style.left = (window.innerWidth / 2) - (boxWidth / 2) + 'px';
        } else {
            console.log("找不到 .glass-container 元素");
        }
    }

    updateBoxPosition();
    window.onresize = updateBoxPosition;
});

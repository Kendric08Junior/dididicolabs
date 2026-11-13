(function () {
    var root = document.documentElement;

    var burger = document.getElementById('dcBurger');
    var nav = document.getElementById('dcNav');
    if (burger && nav) {
        burger.addEventListener('click', function () {
            var open = nav.classList.toggle('open');
            burger.setAttribute('aria-expanded', open ? 'true' : 'false');
        });
    }

    var themeBtn = document.getElementById('dcTheme');
    if (themeBtn) {
        themeBtn.addEventListener('click', function () {
            var next = root.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';
            root.setAttribute('data-theme', next);
            try { localStorage.setItem('dc-theme', next); } catch (e) { }
        });
    }
})();

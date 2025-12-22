(function () {
    const main = document.querySelector('.app-main');
    const bg = document.querySelector('.app-main .space-background');
    if (!main || !bg) return;

    function syncHeight() {
        // scrollHeight includes the full content height (incl. footer inside main)
        const h = Math.max(main.scrollHeight, main.clientHeight);
        bg.style.height = `${h}px`;
    }

    // Initial
    syncHeight();

    // Update on resize and whenever the main scroll container changes size.
    window.addEventListener('resize', syncHeight);

    // ResizeObserver handles most dynamic height changes.
    if ('ResizeObserver' in window) {
        const ro = new ResizeObserver(syncHeight);
        ro.observe(main);
        const inner = main.querySelector('.main-inner');
        if (inner) ro.observe(inner);
    }

    // Also update after fonts load (can affect layout)
    if (document.fonts && document.fonts.ready) {
        document.fonts.ready.then(syncHeight).catch(() => {});
    }
})();

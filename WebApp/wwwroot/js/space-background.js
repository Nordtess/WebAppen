(function () {
    function initSpaceSystem() {
        const main = document.querySelector('.app-main');
        const bg = document.querySelector('.app-main .space-background');
        const container = document.getElementById('starsContainer');
        if (!main || !bg || !container) return;

        // Clear any existing stars
        container.innerHTML = '';

        function syncHeight() {
            // scrollHeight includes the full content height (incl. footer inside main)
            const h = Math.max(main.scrollHeight, main.clientHeight);
            bg.style.height = `${h}px`;
            container.style.height = `${h}px`;
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
            document.fonts.ready.then(syncHeight).catch(() => { });
        }

        const rand = (min, max) => min + Math.random() * (max - min);

        // Use pixel height so star distribution covers the whole page (header->footer)
        const hPx = Math.max(main.scrollHeight, main.clientHeight);

        // 1. Generate Static Stars (250+ for deep space feel)
        const sizes = ['star-tiny', 'star-small', 'star-medium', 'star-large'];
        for (let i = 0; i < 250; i++) {
            const star = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
            star.classList.add('laser-star', sizes[Math.floor(Math.random() * sizes.length)]);
            star.setAttribute('viewBox', '0 0 20 20');

            // Spread across full height; use px for y means true full-scroll coverage.
            star.style.left = `${Math.random() * 100}%`;
            star.style.top = `${Math.random() * hPx}px`;
            star.style.animationDuration = `${rand(2, 7)}s`;
            star.style.animationDelay = `${rand(0, 5)}s`;

            const use = document.createElementNS('http://www.w3.org/2000/svg', 'use');
            use.setAttributeNS('http://www.w3.org/1999/xlink', 'href', '#complexStarShape');
            star.appendChild(use);
            container.appendChild(star);
        }

        // 2. Generate Multiple Shooting Stars
        for (let i = 0; i < 6; i++) {
            const sStar = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
            sStar.classList.add('shooting-star');
            sStar.setAttribute('width', '250');
            sStar.setAttribute('height', '4');

            sStar.style.left = `${Math.random() * 100}%`;
            sStar.style.top = `${Math.random() * hPx}px`;

            const interval = rand(8, 20); // around ~10s-ish, varied
            sStar.style.animation = `shootingStarAnim ${interval}s linear infinite`;
            sStar.style.animationDelay = `${rand(0, 15)}s`;

            const rect = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            rect.setAttribute('width', '250');
            rect.setAttribute('height', '4');
            rect.setAttribute('fill', 'url(#shootingTail)');
            sStar.appendChild(rect);

            container.appendChild(sStar);
        }
    }

    // Initialize on load
    window.addEventListener('DOMContentLoaded', initSpaceSystem);
})();

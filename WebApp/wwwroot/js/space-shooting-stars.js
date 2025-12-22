(function () {
    const main = document.querySelector('.app-main');
    const layer = document.getElementById('shootingStarsLayer');
    if (!main || !layer) return;

    const SHOOT_INTERVAL_MS = 10_000;
    const TRAVEL_MS = 2_000;

    function shootOnce() {
        // `layer` is absolutely positioned to cover the entire scrollable `.app-main`.
        // So position stars in the layer's coordinate system (scroll content space).
        const width = main.clientWidth;
        const height = main.clientHeight;
        const scrollTop = main.scrollTop;

        // Start from top-right of the *visible* viewport within `app-main`.
        const startX = Math.max(0, width - 10);
        const startY = scrollTop + 10;

        // End past the left edge, and down a bit across the current viewport.
        const endX = -120;
        const endY = scrollTop + Math.min(height * 0.6, height - 10);

        const star = document.createElement('img');
        star.src = '/images/svg/space/movingstar.svg';
        star.alt = '';
        star.setAttribute('aria-hidden', 'true');
        star.className = 'moving-star';

        const size = 42;
        star.style.width = `${size}px`;
        star.style.height = `${size}px`;

        // Use translate so we can animate smoothly without reflow.
        star.style.transform = `translate(${startX}px, ${startY}px)`;

        layer.appendChild(star);

        requestAnimationFrame(() => {
            star.style.transition = `transform ${TRAVEL_MS}ms linear`;
            star.style.transform = `translate(${endX}px, ${endY}px)`;
        });

        window.setTimeout(() => {
            star.remove();
        }, TRAVEL_MS + 100);
    }

    shootOnce();
    window.setInterval(shootOnce, SHOOT_INTERVAL_MS);
})();

(function () {
    const main = document.querySelector('.app-main');
    const layer = document.getElementById('shootingStarsLayer');
    if (!main || !layer) return;

    // Prevent multiple instances if the script is loaded twice.
    if (window.__spaceMovingStarTimer) {
        clearTimeout(window.__spaceMovingStarTimer);
        window.__spaceMovingStarTimer = null;
    }

    const SHOOT_INTERVAL_MS = 10_000;
    const TRAVEL_MS = 3_500;

    const GROUP_COUNT = 5;
    const GROUP_STAGGER_MS = 140;

    function rand(min, max) {
        return min + Math.random() * (max - min);
    }

    function shootOne({ startX, startY, endX, endY, size }) {
        const star = document.createElement('img');
        star.src = '/images/svg/space/movingstar.svg';
        star.alt = '';
        star.setAttribute('aria-hidden', 'true');
        star.className = 'moving-star';

        star.style.width = `${size}px`;
        star.style.height = `${size}px`;

        layer.appendChild(star);

        const anim = star.animate(
            [
                { transform: `translate3d(${startX}px, ${startY}px, 0)` },
                { transform: `translate3d(${endX}px, ${endY}px, 0)` }
            ],
            {
                duration: TRAVEL_MS,
                easing: 'linear',
                fill: 'forwards'
            }
        );

        anim.addEventListener('finish', () => {
            star.remove();
        });

        // Fallback cleanup in case finish doesn't fire.
        window.setTimeout(() => {
            star.remove();
        }, TRAVEL_MS + 250);
    }

    function shootGroup() {
        const width = main.clientWidth;
        const height = main.clientHeight;
        const scrollTop = main.scrollTop;

        // Base line: top-right of the current viewport.
        const baseStartX = Math.max(0, width - 10);
        const baseStartY = scrollTop + 10;

        const baseEndX = -140;
        const baseEndY = scrollTop + Math.min(height * 0.6, height - 10);

        // Offset each star slightly so they travel as a "group".
        // We keep offsets small so the group reads as one cluster.
        for (let i = 0; i < GROUP_COUNT; i++) {
            const delay = i * GROUP_STAGGER_MS + rand(0, 60);

            const laneOffsetX = i * 14 + rand(-4, 6);
            const laneOffsetY = i * 10 + rand(-4, 10);

            const size = Math.round(rand(28, 48) * (1 - i * 0.05));

            window.setTimeout(() => {
                shootOne({
                    startX: baseStartX + laneOffsetX,
                    startY: baseStartY + laneOffsetY,
                    endX: baseEndX + laneOffsetX,
                    endY: baseEndY + laneOffsetY,
                    size
                });
            }, delay);
        }
    }

    function scheduleNext(delayMs) {
        window.__spaceMovingStarTimer = window.setTimeout(() => {
            shootGroup();
            scheduleNext(SHOOT_INTERVAL_MS);
        }, delayMs);
    }

    // First group immediately, then every 10s.
    shootGroup();
    scheduleNext(SHOOT_INTERVAL_MS);
})();

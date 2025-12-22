(function () {
    const main = document.querySelector('.app-main');
    const bg = document.querySelector('.app-main .space-background');
    const container = document.getElementById('starsContainer');
    if (!main || !bg || !container) return;

    function syncHeight() {
        const h = Math.max(main.scrollHeight, main.clientHeight);
        bg.style.height = `${h}px`;
        container.style.height = `${h}px`;
        return h;
    }

    const rand = (min, max) => min + Math.random() * (max - min);

    function createStar({ hPx, size, topPx, leftPct, useId, viewBox, extraClass }) {
        const star = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        star.classList.add('laser-star');
        if (extraClass) star.classList.add(extraClass);

        star.setAttribute('width', String(size));
        star.setAttribute('height', String(size));
        star.setAttribute('viewBox', viewBox);

        star.style.left = `${leftPct}%`;
        star.style.top = `${topPx}px`;
        star.style.animationDuration = `${rand(3.5, 7.5)}s`;
        star.style.animationDelay = `${rand(0, 6)}s`;

        const use = document.createElementNS('http://www.w3.org/2000/svg', 'use');
        use.setAttribute('href', useId);
        star.appendChild(use);

        // Small perf hint: avoid forcing layout
        container.appendChild(star);
    }

    function initStars() {
        container.innerHTML = '';

        const hPx = syncHeight();

        // Bulk (4-point stars)
        const STAR_COUNT = 120;
        const sizes = [18, 20, 22, 24, 26, 28, 30, 35];

        for (let i = 0; i < STAR_COUNT; i++) {
            const size = sizes[Math.floor(Math.random() * sizes.length)];
            createStar({
                hPx,
                size,
                topPx: Math.random() * hPx,
                leftPct: Math.random() * 100,
                useId: '#northStarShape',
                viewBox: '0 0 20 20'
            });
        }

        // A few special/bigger stars (complex path) for visual interest.
        // Keep this low to avoid performance issues.
        const SPECIAL_COUNT = 10;
        for (let i = 0; i < SPECIAL_COUNT; i++) {
            createStar({
                hPx,
                size: Math.floor(rand(52, 92)),
                topPx: Math.random() * hPx,
                leftPct: Math.random() * 100,
                useId: '#geminiStarShape',
                // important: match the "better" viewBox you provided
                viewBox: '155.8 111.4 407.4 407.6'
            });
        }
    }

    function createShootingStar(hPx, baseTop, baseRight, offsetIndex) {
        const sStar = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        sStar.classList.add('shooting-star', 'is-laser');
        sStar.setAttribute('width', '220');
        sStar.setAttribute('height', '4');
        sStar.setAttribute('viewBox', '0 0 220 4');

        // Small random offsets so they feel grouped
        sStar.style.top = `${baseTop + rand(-40, 40)}px`;
        sStar.style.right = `${baseRight + rand(-3, 8)}%`;

        // Slow travel across the page
        const dur = rand(6, 10); // seconds
        const delay = rand(0, 0.8) + offsetIndex * rand(0.15, 0.35);

        // Run once; remove after animation
        sStar.style.animation = `shootingStarAnim ${dur}s linear 1`;
        sStar.style.animationDelay = `${delay}s`;

        const rect = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        rect.setAttribute('width', '220');
        rect.setAttribute('height', '4');
        rect.setAttribute('fill', 'url(#shootingStarGradient)');
        rect.setAttribute('filter', 'url(#whiteLaserGlow)');
        sStar.appendChild(rect);

        sStar.addEventListener('animationend', () => {
            sStar.remove();
        });

        container.appendChild(sStar);
    }

    function createShootingStarGroup() {
        const hPx = syncHeight();
        const groupCount = Math.floor(rand(3, 6)); // 3-5

        // Random start point across entire scroll height
        const baseTop = rand(0, hPx);
        const baseRight = -10 - rand(0, 30);

        for (let i = 0; i < groupCount; i++) {
            createShootingStar(hPx, baseTop, baseRight, i);
        }
    }

    let groupTimerId = 0;

    function startShootingStarGroups() {
        window.setTimeout(createShootingStarGroup, 1200);
        groupTimerId = window.setInterval(() => {
            createShootingStarGroup();
        }, 10000);
    }

    // Initial
    initStars();
    startShootingStarGroups();

    // Keep height in sync
    window.addEventListener('resize', () => {
        syncHeight();
    });

    if ('ResizeObserver' in window) {
        const ro = new ResizeObserver(() => {
            syncHeight();
        });
        ro.observe(main);
        const inner = main.querySelector('.main-inner');
        if (inner) ro.observe(inner);
    }

    if (document.fonts && document.fonts.ready) {
        document.fonts.ready.then(() => syncHeight()).catch(() => { });
    }
})();

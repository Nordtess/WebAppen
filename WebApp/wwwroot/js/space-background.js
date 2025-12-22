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

    // Deterministic pseudo-random generator (fast, repeatable)
    function mulberry32(seed) {
        return function () {
            let t = (seed += 0x6D2B79F5);
            t = Math.imul(t ^ (t >>> 15), t | 1);
            t ^= t + Math.imul(t ^ (t >>> 7), t | 61);
            return ((t ^ (t >>> 14)) >>> 0) / 4294967296;
        };
    }

    // Stable seed so stars appear in the same positions each load.
    // (Change this number if you ever want a new "sky" layout.)
    const prng = mulberry32(1337);
    const rand = (min, max) => min + prng() * (max - min);

    function createStar({ size, topPx, leftPct, useId, viewBox }) {
        const star = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        star.classList.add('laser-star');

        star.setAttribute('width', String(size));
        star.setAttribute('height', String(size));
        star.setAttribute('viewBox', viewBox);

        star.style.left = `${leftPct}%`;
        star.style.top = `${topPx}px`;
        // twinkle timing is also deterministic now
        star.style.animationDuration = `${rand(3.5, 7.5)}s`;
        star.style.animationDelay = `${rand(0, 6)}s`;

        const use = document.createElementNS('http://www.w3.org/2000/svg', 'use');
        use.setAttribute('href', useId);
        star.appendChild(use);

        container.appendChild(star);
    }

    function initStars() {
        container.innerHTML = '';

        const hPx = syncHeight();

        // Bulk (4-point stars)
        const STAR_COUNT = 120;
        const sizes = [18, 20, 22, 24, 26, 28, 30, 35];

        for (let i = 0; i < STAR_COUNT; i++) {
            const size = sizes[Math.floor(prng() * sizes.length)];
            createStar({
                size,
                topPx: prng() * hPx,
                leftPct: prng() * 100,
                useId: '#northStarShape',
                viewBox: '0 0 20 20'
            });
        }

        // Special/bigger stars (complex path)
        const SPECIAL_COUNT = 10;
        for (let i = 0; i < SPECIAL_COUNT; i++) {
            createStar({
                size: Math.floor(rand(52, 92)),
                topPx: prng() * hPx,
                leftPct: prng() * 100,
                useId: '#geminiStarShape',
                viewBox: '155.8 111.4 407.4 407.6'
            });
        }
    }

    function createShootingStar(baseTop, baseRight, offsetIndex) {
        const sStar = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        sStar.classList.add('shooting-star', 'is-laser');
        sStar.setAttribute('width', '220');
        sStar.setAttribute('height', '4');
        sStar.setAttribute('viewBox', '0 0 220 4');

        sStar.style.top = `${baseTop + rand(-40, 40)}px`;
        sStar.style.right = `${baseRight + rand(-3, 8)}%`;

        // Slow travel across the page
        const dur = rand(6, 10);
        const delay = rand(0, 0.8) + offsetIndex * rand(0.15, 0.35);

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

    // Create a group within the current viewport so users will actually see it.
    function createShootingStarGroupInViewport() {
        syncHeight();

        const viewTop = main.scrollTop;
        const viewH = main.clientHeight;
        const groupCount = Math.floor(rand(3, 6));

        // Start somewhere visible with a little padding
        const pad = 60;
        const baseTop = rand(viewTop + pad, Math.max(viewTop + pad, viewTop + viewH - pad));
        const baseRight = -10 - rand(0, 20);

        for (let i = 0; i < groupCount; i++) {
            createShootingStar(baseTop, baseRight, i);
        }
    }

    function startShootingStarGroups() {
        window.setTimeout(createShootingStarGroupInViewport, 1200);
        window.setInterval(() => {
            createShootingStarGroupInViewport();
        }, 10000);
    }

    initStars();
    startShootingStarGroups();

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

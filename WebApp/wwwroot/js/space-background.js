(function () {
    // Skapar och renderar statiska "stjärnor" i bakgrunden som täcker .app-main höjd.
    const main = document.querySelector('.app-main');
    const bg = document.querySelector('.app-main .space-background');
    const container = document.getElementById('starsContainer');
    if (!main || !bg || !container) return;

    // Synkronisera bakgrundselementens höjd med huvudens innehållshöjd
    function syncHeight() {
        const h = Math.max(main.scrollHeight, main.clientHeight);
        bg.style.height = `${h}px`;
        container.style.height = `${h}px`;
        return h;
    }

    // Enkel deterministisk PRNG (mulberry32) för stabil placering mellan sidladdningar
    function mulberry32(seed) {
        return function () {
            let t = (seed += 0x6D2B79F5);
            t = Math.imul(t ^ (t >>> 15), t | 1);
            t ^= t + Math.imul(t ^ (t >>> 7), t | 61);
            return ((t ^ (t >>> 14)) >>> 0) / 4294967296;
        };
    }

    const prng = mulberry32(1337);
    const rand = (min, max) => min + prng() * (max - min);

    function clamp(v, min, max) {
        return Math.max(min, Math.min(max, v));
    }

    // Skapa ett SVG-element för en stjärna och lägg till i container
    function createStar({ size, topPx, leftPct, useId, viewBox }) {
        const star = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        star.classList.add('laser-star');

        star.setAttribute('width', String(size));
        star.setAttribute('height', String(size));
        star.setAttribute('viewBox', viewBox);

        star.style.left = `${leftPct}%`;
        star.style.top = `${topPx}px`;

        const use = document.createElementNS('http://www.w3.org/2000/svg', 'use');
        use.setAttribute('href', useId);
        star.appendChild(use);

        container.appendChild(star);
    }

    function initStars() {
        container.innerHTML = '';

        const hPx = syncHeight();
        const isMobile = window.matchMedia && window.matchMedia('(max-width: 800px)').matches;

        // Höjdbaserad densitetsmodell: antal stjärnor per 1000px sida.
        // Begränsa antal för att undvika överbelastning på korta eller mycket långa sidor.
        const densityPer1000 = isMobile ? 38 : 48;
        const minStars = isMobile ? 35 : 50;
        const maxStars = isMobile ? 80 : 130;

        const specialDensityPer1000 = isMobile ? 2.5 : 5;
        const minSpecial = isMobile ? 2 : 4;
        const maxSpecial = isMobile ? 5 : 10;

        const STAR_COUNT = clamp(Math.round(densityPer1000 * (hPx / 1000)), minStars, maxStars);
        const SPECIAL_COUNT = clamp(Math.round(specialDensityPer1000 * (hPx / 1000)), minSpecial, maxSpecial);

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

        for (let i = 0; i < SPECIAL_COUNT; i++) {
            createStar({
                size: Math.floor(rand(52, 92)),
                topPx: prng() * hPx,
                leftPct: prng() * 100,
                useId: '#geminiStarShape',
                viewBox: '155.8 111.4 407.4 407.6'
            });
        }

        // Förbättra visuell start genom att tvinga layout och använda negativa delays
        // så att varje stjärna visas mitt i sin animation istället för alla från 0%.
        const stars = container.querySelectorAll('.laser-star');
        void container.offsetHeight;

        for (const star of stars) {
            const duration = rand(2.4, 5.6);
            star.style.animationDuration = `${duration}s`;
            star.style.animationDelay = `${-rand(0, duration)}s`;
        }
    }

    initStars();

    window.addEventListener('resize', () => {
        syncHeight();
        initStars();
    });

    // Om ResizeObserver finns, uppdatera höjd när main eller dess inner förändras
    if ('ResizeObserver' in window) {
        const ro = new ResizeObserver(() => {
            syncHeight();
        });
        ro.observe(main);
        const inner = main.querySelector('.main-inner');
        if (inner) ro.observe(inner);
    }

    // När webfont-laddning är klar: justera höjden (failsafe catch för äldre UA)
    if (document.fonts && document.fonts.ready) {
        document.fonts.ready.then(() => syncHeight()).catch(() => { });
    }
})();

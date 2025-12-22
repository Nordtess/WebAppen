(function () {
    const main = document.querySelector('.app-main');
    const container = document.getElementById('starsContainer');
    if (!main || !container) return;

    const rand = (min, max) => min + Math.random() * (max - min);

    function createShootingStarFromViewportTopRight(viewTop, viewW) {
        const sStar = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        sStar.classList.add('shooting-star', 'is-laser');

        // Wider canvas to support a long wedge tail
        sStar.setAttribute('width', '400');
        sStar.setAttribute('height', '100');
        sStar.setAttribute('viewBox', '0 0 400 100');

        // SPAWN: top-right of the CURRENT viewport
        // Use left positioning to make animation math consistent.
        const startLeft = viewW - 40; // near the right edge
        sStar.style.top = `${viewTop + 20}px`;
        sStar.style.left = `${startLeft}px`;

        // Smooth glide duration (2.5 - 4.0s)
        const dur = rand(2.5, 4.0);
        sStar.style.animation = `shootingStarGlide ${dur}s linear forwards`;

        // Tail group so we can apply flutter on the whole tail
        const tailGroup = document.createElementNS('http://www.w3.org/2000/svg', 'g');
        tailGroup.classList.add('shooting-star-tail-group');
        sStar.appendChild(tailGroup);

        // Aura (wide WHITE wedge, no mint)
        const aura = document.createElementNS('http://www.w3.org/2000/svg', 'polygon');
        aura.setAttribute('points', '25,50 350,40 350,60');
        aura.setAttribute('fill', 'rgba(248, 248, 255, 0.40)');
        aura.setAttribute('filter', 'url(#whiteLaserGlow)');
        tailGroup.appendChild(aura);

        // Core (thin WHITE wedge)
        const core = document.createElementNS('http://www.w3.org/2000/svg', 'polygon');
        core.setAttribute('points', '25,50 300,47 300,53');
        core.setAttribute('fill', '#F8F8FF');
        core.setAttribute('filter', 'url(#whiteLaserGlow)');
        tailGroup.appendChild(core);

        // Stardust particles (WHITE)
        for (let i = 0; i < 6; i++) {
            const particle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            particle.classList.add('stardust-particle');
            particle.setAttribute('cx', String(60 + (i * 55)));
            particle.setAttribute('cy', String(50 + (rand(-6, 6))));
            particle.setAttribute('r', String(1.5 + rand(0, 1.5)));
            particle.style.animationDelay = `${i * 0.06}s`;
            sStar.appendChild(particle);
        }

        // Head star at x=0 (leads movement)
        const head = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        head.setAttribute('x', '0');
        head.setAttribute('y', '37');
        head.setAttribute('width', '26');
        head.setAttribute('height', '26');
        head.setAttribute('viewBox', '0 0 20 20');
        head.setAttribute('filter', 'url(#whiteLaserGlow)');

        const headUse = document.createElementNS('http://www.w3.org/2000/svg', 'use');
        headUse.setAttribute('href', '#northStarShape');
        head.appendChild(headUse);
        sStar.appendChild(head);

        sStar.addEventListener('animationend', () => sStar.remove());
        container.appendChild(sStar);
    }

    function createShootingStarGroupInViewport() {
        const viewTop = main.scrollTop;
        const viewW = main.clientWidth;

        // 1-3 stars per group (spawn origin is fixed; avoid stacking too much)
        const groupCount = Math.floor(rand(1, 4));
        for (let i = 0; i < groupCount; i++) {
            // 550ms - 950ms spacing between stars
            window.setTimeout(() => {
                createShootingStarFromViewportTopRight(viewTop, viewW);
            }, i * rand(550, 950));
        }
    }

    window.setTimeout(createShootingStarGroupInViewport, 1200);
    window.setInterval(createShootingStarGroupInViewport, 10000);
})();

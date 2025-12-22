(function () {
    const main = document.querySelector('.app-main');
    const container = document.getElementById('starsContainer');
    if (!main || !container) return;

    const rand = (min, max) => min + Math.random() * (max - min);

    function createShootingStarFromTop(viewTop, viewW) {
        const sStar = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        sStar.classList.add('shooting-star', 'is-laser');

        // Wider canvas to support a long wedge tail
        sStar.setAttribute('width', '400');
        sStar.setAttribute('height', '100');
        sStar.setAttribute('viewBox', '0 0 400 100');

        // Spawn just above the current viewport
        const startLeft = rand(0, Math.max(0, viewW));
        sStar.style.top = `${viewTop - 100}px`;
        sStar.style.left = `${startLeft}px`;

        // Smooth glide duration (2.5 - 3.5s)
        const dur = rand(2.5, 3.5);
        sStar.style.animation = `shootingStarGlide ${dur}s linear forwards`;

        // Tail group so we can apply flutter on the whole tail
        const tailGroup = document.createElementNS('http://www.w3.org/2000/svg', 'g');
        tailGroup.classList.add('shooting-star-tail-group');
        sStar.appendChild(tailGroup);

        // Aura (wide mint wedge)
        const aura = document.createElementNS('http://www.w3.org/2000/svg', 'polygon');
        aura.setAttribute('points', '20,50 380,20 380,80');
        aura.setAttribute('fill', 'url(#shootingStarMintGradient)');
        aura.setAttribute('filter', 'url(#mintLaserGlow)');
        aura.setAttribute('opacity', '0.5');
        tailGroup.appendChild(aura);

        // Core (thin white wedge)
        const core = document.createElementNS('http://www.w3.org/2000/svg', 'polygon');
        core.setAttribute('points', '20,50 360,40 360,60');
        core.setAttribute('fill', '#F8F8FF');
        core.setAttribute('filter', 'url(#whiteLaserGlow)');
        tailGroup.appendChild(core);

        // Stardust particles (trail behind head)
        for (let i = 1; i <= 5; i++) {
            const particle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            particle.classList.add('stardust-particle');
            particle.setAttribute('cx', String(30 + i * 50));
            particle.setAttribute('cy', String(50 + (rand(-10, 10))));
            particle.setAttribute('r', String(1.5 + rand(0, 2.5)));
            particle.style.animationDelay = `${i * 0.1}s`;
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

        // 2-4 stars per group (avoid clutter)
        const groupCount = Math.floor(rand(2, 5));
        for (let i = 0; i < groupCount; i++) {
            // 600ms - 1000ms spacing between stars
            window.setTimeout(() => {
                createShootingStarFromTop(viewTop, viewW);
            }, i * rand(600, 1000));
        }
    }

    window.setTimeout(createShootingStarGroupInViewport, 1200);
    window.setInterval(createShootingStarGroupInViewport, 10000);
})();

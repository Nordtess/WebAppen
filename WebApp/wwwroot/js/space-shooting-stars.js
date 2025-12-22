(function () {
    const main = document.querySelector('.app-main');
    const container = document.getElementById('starsContainer');
    if (!main || !container) return;

    const rand = (min, max) => min + Math.random() * (max - min);

    function createShootingStarFromTop(viewTop, viewW) {
        const sStar = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        sStar.classList.add('shooting-star', 'is-laser');

        // Canvas size (head + tail)
        sStar.setAttribute('width', '300');
        sStar.setAttribute('height', '50');
        sStar.setAttribute('viewBox', '0 0 300 50');

        // Spawn at the TOP of the current viewport
        const startTop = viewTop - 50;
        const startLeft = rand(0, Math.max(0, viewW));

        sStar.style.top = `${startTop}px`;
        sStar.style.left = `${startLeft}px`;

        // Smooth glide: 2.5s - 3.5s
        const dur = rand(2.5, 3.5);
        sStar.style.animation = `shootingStarGlide ${dur}s linear forwards`;

        // HEAD first at x=0 (leads)
        const head = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        head.setAttribute('x', '0');
        head.setAttribute('y', '12');
        head.setAttribute('width', '26');
        head.setAttribute('height', '26');
        head.setAttribute('viewBox', '0 0 20 20');
        head.setAttribute('filter', 'url(#whiteLaserGlow)');

        const headUse = document.createElementNS('http://www.w3.org/2000/svg', 'use');
        headUse.setAttribute('href', '#northStarShape');
        head.appendChild(headUse);
        sStar.appendChild(head);

        // TAIL after head at x=20 (behind)
        const rect = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        rect.setAttribute('x', '20');
        rect.setAttribute('y', '23');
        rect.setAttribute('width', '250');
        rect.setAttribute('height', '4');
        rect.setAttribute('fill', 'url(#shootingStarGradient)');
        rect.setAttribute('filter', 'url(#whiteLaserGlow)');
        sStar.appendChild(rect);

        sStar.addEventListener('animationend', () => { sStar.remove(); });
        container.appendChild(sStar);
    }

    function createShootingStarGroupInViewport() {
        const viewTop = main.scrollTop;
        const viewW = main.clientWidth;

        // 2-4 stars per group (less clutter)
        const groupCount = Math.floor(rand(2, 5));
        for (let i = 0; i < groupCount; i++) {
            // 600ms - 1000ms between stars in a group
            window.setTimeout(() => {
                createShootingStarFromTop(viewTop, viewW);
            }, i * rand(600, 1000));
        }
    }

    window.setTimeout(createShootingStarGroupInViewport, 1200);
    window.setInterval(createShootingStarGroupInViewport, 10000);
})();

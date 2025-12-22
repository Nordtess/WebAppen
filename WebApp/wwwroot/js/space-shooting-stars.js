(function () {
    const main = document.querySelector('.app-main');
    const container = document.getElementById('starsContainer');
    if (!main || !container) return;

    const rand = (min, max) => min + Math.random() * (max - min);

    function createShootingStarFromViewport() {
        const viewTop = main.scrollTop;
        const viewW = main.clientWidth;

        const sStar = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        sStar.classList.add('shooting-star', 'is-laser');
        sStar.setAttribute('width', '400');
        sStar.setAttribute('height', '100');
        sStar.setAttribute('viewBox', '0 0 400 100');

        sStar.style.top = `${viewTop + 20}px`;
        sStar.style.left = `${viewW - 20}px`;

        const dur = 5.0 + Math.random() * 3.0;
        sStar.style.animation = `shootingStarGlide ${dur}s linear forwards`;

        const tailGroup = document.createElementNS('http://www.w3.org/2000/svg', 'g');
        tailGroup.classList.add('shooting-star-tail-group');
        sStar.appendChild(tailGroup);

        const aura = document.createElementNS('http://www.w3.org/2000/svg', 'polygon');
        aura.setAttribute('points', '25,50 350,35 350,65');
        aura.setAttribute('fill', 'rgba(248, 248, 255, 0.3)');
        aura.setAttribute('filter', 'url(#whiteLaserGlow)');
        tailGroup.appendChild(aura);

        const core = document.createElementNS('http://www.w3.org/2000/svg', 'polygon');
        core.setAttribute('points', '25,50 300,47 300,53');
        core.setAttribute('fill', '#F8F8FF');
        core.setAttribute('filter', 'url(#whiteLaserGlow)');
        tailGroup.appendChild(core);

        const head = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        head.setAttribute('x', '0');
        head.setAttribute('y', '37');
        head.setAttribute('width', '26');
        head.setAttribute('height', '26');
        head.setAttribute('filter', 'url(#whiteLaserGlow)');

        const headUse = document.createElementNS('http://www.w3.org/2000/svg', 'use');
        headUse.setAttribute('href', '#northStarShape');
        head.appendChild(headUse);
        sStar.appendChild(head);

        sStar.addEventListener('animationend', () => sStar.remove());
        container.appendChild(sStar);
    }

    window.setInterval(createShootingStarFromViewport, 9000);
})();

(function () {
    const main = document.querySelector('.app-main');
    const container = document.getElementById('starsContainer');
    if (!main || !container) return;

    const rand = (min, max) => min + Math.random() * (max - min);

    function createShootingStarFromViewport() {
        const rect = container.getBoundingClientRect();
        const viewW = rect.width;
        const viewH = main.clientHeight;
        const viewTop = main.scrollTop;

        const sStar = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        sStar.classList.add('shooting-star', 'is-laser');
        sStar.setAttribute('width', '400');
        sStar.setAttribute('height', '100');
        sStar.setAttribute('viewBox', '0 0 400 100');

        const spawnPadding = 16;
        const spawnX = Math.max(spawnPadding, viewW - 400 - spawnPadding);
        const spawnY = viewTop + rand(10, Math.min(140, Math.max(60, viewH * 0.35)));

        sStar.style.left = `${spawnX}px`;
        sStar.style.top = `${spawnY}px`;

        const dx = -(viewW + 400 + 900);
        const dy = (viewH + 900);
        sStar.style.setProperty('--dx', `${dx}px`);
        sStar.style.setProperty('--dy', `${dy}px`);

        const rot = 155 + rand(-6, 6);
        sStar.style.setProperty('--rot', `${rot}deg`);

        const dur = 5.0 + Math.random() * 3.0;
        sStar.style.animation = `shootingStarGlide ${dur}s linear forwards`;

        const tailGroup = document.createElementNS('http://www.w3.org/2000/svg', 'g');
        tailGroup.classList.add('shooting-star-tail-group');
        sStar.appendChild(tailGroup);

        const HEAD_X = 370;

        const aura = document.createElementNS('http://www.w3.org/2000/svg', 'polygon');
        aura.setAttribute('points', `${HEAD_X},50 20,35 20,65`);
        aura.setAttribute('fill', 'rgba(248, 248, 255, 0.3)');
        aura.setAttribute('filter', 'url(#whiteLaserGlow)');
        tailGroup.appendChild(aura);

        const core = document.createElementNS('http://www.w3.org/2000/svg', 'polygon');
        core.setAttribute('points', `${HEAD_X},50 60,47 60,53`);
        core.setAttribute('fill', '#F8F8FF');
        core.setAttribute('filter', 'url(#whiteLaserGlow)');
        tailGroup.appendChild(core);

        for (let i = 1; i <= 5; i++) {
            const p = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            p.classList.add('stardust-particle');
            p.setAttribute('cx', String(HEAD_X - (i * 55)));
            p.setAttribute('cy', String(50 + rand(-7, 7)));
            p.setAttribute('r', '1.5');
            p.style.animationDelay = `${i * 0.12}s`;
            sStar.appendChild(p);
        }

        const head = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        head.setAttribute('x', String(HEAD_X - 13));
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

    window.setInterval(createShootingStarFromViewport, 9000);
})();

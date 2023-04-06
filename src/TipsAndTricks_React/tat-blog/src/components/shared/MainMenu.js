import React from 'react'
import MenuItem from '../controls/MenuItem';

const MainMenu = () => {
    return (
        <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            {/* <li className="nav-item">
                <a className="nav-link active" aria-current="page" href="/">Home</a>
            </li> */}

            <MenuItem link="/posts" title="Posts" />
            <MenuItem link="/contact" title="Contact" />
            <MenuItem link="/about-us" title="About Us" />
            <MenuItem link="/rss" title="RSS" />
        </ul>
    )
}

export default MainMenu;
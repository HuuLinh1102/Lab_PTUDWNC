import React from 'react'
import { NavLink } from 'react-router-dom';

const MenuItem = ({link, title}) => {
  return (
    <li className="nav-item">
        <NavLink className="nav-link" to={link}>
            {title}
        </NavLink>
    </li>
  )
}

export default MenuItem;
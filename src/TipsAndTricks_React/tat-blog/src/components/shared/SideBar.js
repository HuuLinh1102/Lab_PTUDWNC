import React from 'react'
import Categories from '../widgets/Categories';
import Archives from '../widgets/Archives';

const SideBar = () => {
  return (
    <aside className='h-100 p-4 border-end'>
      <Categories />
      <Archives />
    </aside>
  )
}

export default SideBar;
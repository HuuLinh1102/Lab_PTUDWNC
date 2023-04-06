import React from 'react'
import Footer from './Footer';
import Header from './Header';
import SideBar from './SideBar';
import WidgetPane from './WidgetPane';

const Layout = ({children}) => {
  return (
    <>
      <Header />

      <div className='container-fluid'>
        <div className="row">
          <div className="col-2">
            <SideBar />
          </div>
          <div className="col-7">
            <main className='py-4'>
              {children}
            </main>
          </div>
          <div className="col-3">
            <WidgetPane />
          </div>
        </div>
      </div>

      <Footer />
    </>
  )
}

export default Layout;
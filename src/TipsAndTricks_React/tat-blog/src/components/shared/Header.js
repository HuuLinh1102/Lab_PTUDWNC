import React from 'react'
import MainMenu from './MainMenu';
import SearchForm from '../widgets/SearchForm';

const Header = () => {
    return (
        <header className='bg-secondary sticky-top'>
            <nav className="container-fluid navbar navbar-expand-lg navbar-dark">
                <div className="container-fluid">
                    <a className="navbar-brand" href="/">TAT Blog</a>
                    <button 
                        className="navbar-toggler" 
                        type="button" 
                        data-bs-toggle="collapse" 
                        data-bs-target="#navbarSupportedContent" 
                        aria-controls="navbarSupportedContent" 
                        aria-expanded="false" 
                        aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div 
                        className="collapse navbar-collapse" 
                        id="navbarSupportedContent">
                        <MainMenu />
                        <SearchForm />
                    </div>
                </div>
            </nav>
        </header>
    )
}

export default Header;
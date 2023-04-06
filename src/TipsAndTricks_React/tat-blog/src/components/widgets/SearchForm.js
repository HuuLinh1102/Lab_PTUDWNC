import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';

const SearchForm = () => {
    const navigate = useNavigate();
    const [searchTerm, setSearchTerm] = useState('');

    function submitSearchTerm(evt) {
        evt.preventDefault();
        const keyword = searchTerm.trim();

        if (keyword.length) {
            navigate(`/posts?keyword=${keyword}`)
            setSearchTerm('');
        }
    }

    return (
        <form 
            className="d-flex" 
            role="search"
            onSubmit={submitSearchTerm}>
            <input 
                className="form-control me-2" 
                type="search" 
                placeholder="Search" 
                aria-label="Search" 
                value={searchTerm}
                onChange={(evt) => setSearchTerm(evt.target.value)}
            />
            <button 
                className="btn btn-outline-warning text-white" 
                type="submit"
            >
                Search
            </button>
        </form>
    )
}

export default SearchForm;
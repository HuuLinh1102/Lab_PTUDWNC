import { format } from 'date-fns';
import React, { useEffect, useState } from 'react'

const BestAuthors = () => {
  const [authors, setAuthors] = useState([]);

  useEffect(() => {
    loadBestAuthors();

   async function loadBestAuthors() {
    const response = await fetch('https://localhost:7085/api/authors/best/5');
    const data = await response.json();

    setAuthors(data.result);
   } 
  }, []);

  return (
    <div className='mb-5'>
      <h1 className='text-center text-success mb-4'>
        Best Authors
      </h1>

      <div className="card-group">
        {authors.map(item => (
          <div key={item.id} className="card">
            <img 
              src={item.imageUrl || "/images/author.png"} 
              className="card-img-top"
              alt={item.fullName} />

            <div className="card-body">
              <h5 className="card-title">
                {item.fullName}
              </h5>
              <p className="card-text text-primary">
                {item.email}
              </p>
              <p className="card-text mb-0">
                <small className="text-muted">
                  Joined on {format(Date.parse(item.joinedDate), 'MMM dd, yyyy')}
                </small>
              </p>
              <p className="card-text mb-0">
                <small className="text-muted">{item.postCount} articles</small>
              </p>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default BestAuthors;
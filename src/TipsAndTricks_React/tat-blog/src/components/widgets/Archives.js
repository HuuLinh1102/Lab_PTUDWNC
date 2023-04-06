import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom';

const Archives = () => {
  const [monthlyCount, setMonthlyCount] = useState([]);

  useEffect(() => {
    loadArchivedData();

   async function loadArchivedData() {
    const response = await fetch('https://localhost:7085/api/posts/archives/12');
    const data = await response.json();

    setMonthlyCount(data.result);
   } 
  }, []);

  return (
    <div className="mb-3">
      <h4 className='mb-3 text-danger'>
        Archives
      </h4>

      <div className="list-group list-group-flush">
        {monthlyCount.map(item => (
          <Link
            className="list-group-item d-flex justify-content-between align-items-start"
            to={`/archives/${item.year}/${item.month}`}
          >
            <div className="me-auto">
              {item.monthName} {item.year}
            </div>
            <span className="badge bg-primary rounded-pill">
              {item.postCount}
            </span>
          </Link>
        ))}
      </div>
    </div>
  )
}

export default Archives;
import React from 'react'

const NotFound = () => {
  return (
    <div className='text-center'>
        <img
            src='/images/404.jpg'
            alt='Page Node Found'
        />
        <h1 className='text-danger my-5'>
            Page Not Found
        </h1>
    </div>
  )
}

export default NotFound;
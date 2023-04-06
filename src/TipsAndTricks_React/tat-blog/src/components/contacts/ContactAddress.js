import React from 'react'

const ContactAddress = () => {
  return (
    <div className='bg-primary text-white h-100 p-4'>
        <h3>
            Reach Us
        </h3>
        <p>
            Email: admin@tatblog.com
        </p>
        <p>
            Phone: +84987654321
        </p>
        <p>
            Address: 123 Phu Dong Thien Vuong, Ward 8, Da Lat City
        </p>

        <div>
            <img className='w-100' src="/images/map.jpg" alt="Map" />
        </div>
    </div>
  )
}

export default ContactAddress;
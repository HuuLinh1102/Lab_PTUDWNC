import React from 'react'
import ContactAddress from '../components/contacts/ContactAddress';
import ContactForm from '../components/contacts/ContactForm';

const Contact = () => {
  return (
    <div>
      <div className='bg-success mb-5 p-5 text-center'>
        <h1 className='text-info'>Contact Us</h1>
        <p className='text-white'>
          Feel free to get in touch with us.
        </p>
      </div>

      <div className="row">
        <div className="col-7">
          <ContactForm />
        </div>

        <div className="col-5">
          <ContactAddress />
        </div>
      </div>
    </div>
  )
}

export default Contact;
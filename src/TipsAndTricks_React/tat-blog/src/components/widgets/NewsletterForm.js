import React, { useRef, useState } from 'react'

const NewsletterForm = () => {
  const emailRef = useRef();
  const [message, setMessage] = useState('');

  async function submitNewsletter(evt) {
    evt.preventDefault();
    const emailAddress = emailRef.current.value.trim();

    if (emailAddress.length) {
      setMessage('');

      const response = await fetch('https://localhost:7085/api/subscribers', {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({email: emailAddress})
      });
      const data = await response.json();

      if (data.isSuccess) {
        setMessage(`Your email ${emailAddress} has been saved!`);
      }
    } else {
      setMessage('Invalid email address');
    }
  }

  return (
    <div className="mb-4">
      <h4 className='mb-3 text-primary'>
        Newsletter
      </h4>

      <p>
        Do not want to miss any news and updates, then
        please subscribe to our mailing list.
      </p>

      <form onSubmit={submitNewsletter}>
        <div className="mb-3">
          <input 
            ref={emailRef}
            type="email" 
            className="form-control" 
            placeholder='Email address' />
          {message && <p className='text-danger'>{message}</p>}
        </div>

        <button 
          type="submit" 
          className="btn btn-primary">
            Subscribe
        </button>
      </form>
    </div>
  )
}

export default NewsletterForm;
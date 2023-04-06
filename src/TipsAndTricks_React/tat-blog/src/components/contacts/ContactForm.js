import React from 'react'

const ContactForm = () => {
    return (
        <div className='pt-4'>
            <form>
                <h3 className='mb-4 text-success'>
                    Send Your Request
                </h3>

                <div className="row mb-3">
                    <div className="col">
                        <label
                            for="firstName"
                            className="form-label">
                            First Name (*)
                        </label>
                        <input
                            type="text"
                            className="form-control"
                            id="firstName"
                            placeholder="First name"
                            aria-label="First name" />
                    </div>

                    <div className="col">
                        <label
                            for="lastName"
                            className="form-label">
                            First Name (*)
                        </label>
                        <input
                            type="text"
                            className="form-control"
                            id="lastName"
                            placeholder="Last name"
                            aria-label="Last name" />
                    </div>
                </div>

                <div className="mb-3">
                    <label
                        for="email"
                        className="form-label">
                        Email address (*)
                    </label>
                    <input
                        type="email"
                        className="form-control"
                        id="email"
                        placeholder='email@address.com'
                        aria-label='Email' />
                </div>

                <div className="mb-3">
                    <label
                        for="subject"
                        className="form-label">
                        Subject (*)
                    </label>
                    <input
                        type="text"
                        className="form-control"
                        id="subject"
                        placeholder='Subject'
                        aria-label='Subject' />
                </div>

                <div className="mb-3">
                    <label
                        for="message"
                        className="form-label">
                        Details (*)
                    </label>
                    <textarea
                        className='form-control'
                        id='message'
                        placeholder='Message content'
                    >
                    </textarea>
                </div>

                <button type="submit" className="btn btn-warning">Submit</button>
            </form>
        </div>
    )
}

export default ContactForm;
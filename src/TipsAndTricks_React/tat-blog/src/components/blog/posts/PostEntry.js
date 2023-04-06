import React from 'react'

const PostEntry = ({ post }) => {
    const {
        urlSlug,
        title,
        imageUrl,
        category: {name: categoryName},
        author: {fullName: authorName}
    } = post;

    return (
        <div className="card mb-3">
            <div className="row g-0">
                <div className="col-md-3">
                    <img
                        src={imageUrl || "/images/logo192.png"}
                        className="img-fluid rounded-start"
                        alt="Post Entry" />
                </div>
                <div className="col-md-9">
                    <div className="card-body">
                        <a
                            className='card-title text-decoration-none'
                            href={`/post/${urlSlug}`}
                        >
                            {title}
                        </a>
                        <p className="card-text">
                            <small className="text-muted">
                                Category: {categoryName}
                            </small>
                            {" - "}
                            <small className="text-muted">
                                Author: {authorName}
                            </small>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default PostEntry;
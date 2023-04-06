import React from 'react'
import TagList from '../tags/TagList';

const PostItem = ({ post }) => {
    const {
        urlSlug,
        title,
        shortDescription,
        imageUrl,
        category,
        author,
        tags
    } = post;

    return (
        <div className="card h-100">
            <img
                src={imageUrl || "/images/tips.jpg"}
                className="card-img-top"
                alt={title}
                height="150px"
            />
            <div className="card-body">
                <a
                    className='text-decoration-none'
                    href={`/post/${urlSlug}`}
                    title="Read details"
                >
                    <h5 className="card-title">
                        {title}
                    </h5>
                </a>
                <TagList tags={tags} />
                <p className='text-muted mb-0'>
                    Category:
                    <a
                        className='text-decoration-none'
                        href={`/category/${category.urlSlug}`}
                    >
                        {category.name}
                    </a>
                </p>
                <p className='text-muted'>
                    Author:
                    <a
                        className='text-decoration-none'
                        href={`/author/${author.urlSlug}`}
                    >
                        {author.fullName}
                    </a>
                </p>
                <p className="card-text">
                    {shortDescription}
                </p>
            </div>
        </div>
    )
}

export default PostItem;
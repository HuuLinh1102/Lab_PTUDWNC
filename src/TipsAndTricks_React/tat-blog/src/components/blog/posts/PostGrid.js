import React from 'react';
import PostItem from './PostItem';

const PostGrid = ({articles, columns}) => {
    return (
        <div className='container-fluid'>
            <div className={`row row-cols-1 row-cols-md-${columns || 2} g-4`}>
                {articles.map((post) => (
                    <div className="col" key={post.id}>
                        <PostItem post={post} />
                    </div>
                ))}
            </div>
        </div>
    )
}

export default PostGrid;
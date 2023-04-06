import React from 'react'
import { useParams } from 'react-router-dom';
import PostSearch from '../components/blog/posts/PostSearch';

const PostByTag = () => {
  const params = useParams();

  return (
    <div>
      <h1>
        Articles Contain Tag <span className='text-primary'> {params.slug}</span>
      </h1>

      <PostSearch postQuery={{tagSlug: params.slug}} />
    </div>
  )
}

export default PostByTag;
import React from 'react'
import { useParams } from 'react-router-dom';
import PostSearch from '../components/blog/posts/PostSearch';

const PostByCategory = () => {
  const params = useParams();

  return (
    <div>
      <h1>
        Articles In Category <span className='text-primary'> {params.slug}</span>
      </h1>

      <PostSearch postQuery={{categorySlug: params.slug}} />
    </div>
  )
}

export default PostByCategory;
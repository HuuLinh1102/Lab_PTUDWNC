import React from 'react'
import { useParams } from 'react-router-dom';
import PostSearch from '../components/blog/posts/PostSearch';
import AuthorDetail from '../components/blog/authors/AuthorDetail';

const PostByAuthor = () => {
  const params = useParams();

  return (
    <div>
      <h1 className='mb-5'>
        Articles Written By Author <span className='text-primary'> {params.slug}</span>
      </h1>

      {/* Hien thi thong tin tac gia */}
      <AuthorDetail authorSlug={params.slug} />

      <PostSearch postQuery={{authorSlug: params.slug}} />
    </div>
  )
}

export default PostByAuthor;
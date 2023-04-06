import React, { useState } from 'react'
import { useLocation } from 'react-router-dom';
import PostSearch from '../components/blog/posts/PostSearch';
import PostItem from '../components/blog/posts/PostItem';

const Blog = () => {
  const queryStrings = new URLSearchParams(useLocation().search);
  const keyword = queryStrings.get('keyword');
  const [postList, setPostList] = useState([]);

  if (postList.length > 0)
    return (
      <div className='p-5'>
        {postList.map(item => {
          return (
            <PostItem PostItem={item} />
          );
        })};
      </div>
    );
    else return (
      <></>
    );
  return (
    <div>
      <h1 className='mb-5'>
        {keyword ? (
          <>
            Search results for keyword:
            <span className='text-danger'> {keyword}</span>
          </>
        ) : "Latest Posts"}

      </h1>

      <PostSearch postQuery={{keyword: keyword}} />
    </div>
    
  )
}

export default Blog;
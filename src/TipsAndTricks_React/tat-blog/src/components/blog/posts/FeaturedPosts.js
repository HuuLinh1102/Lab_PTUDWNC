import React, { useEffect, useState } from 'react'
import PostGrid from './PostGrid';

const FeaturedPosts = () => {
  const [topPosts, setTopPosts] = useState([]);

  useEffect(() => {
    loadFeaturedPosts();

   async function loadFeaturedPosts() {
    const response = await fetch('https://localhost:7085/api/posts/featured/3');
    const data = await response.json();

    setTopPosts(data.result);
   } 
  }, []);

  return (
    <div className='mb-5'>
      <h1 className='text-center text-primary mb-4'>
        Featured Posts
      </h1>

      <PostGrid columns={3} articles={topPosts} />
    </div>
  )
}

export default FeaturedPosts;
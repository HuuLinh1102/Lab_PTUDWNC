import React from 'react';
import FeaturedPosts from '../components/blog/posts/FeaturedPosts';
import BestAuthors from '../components/widgets/BestAuthors';

const Home = () => {
  return (
    <div>
      <BestAuthors />
      <FeaturedPosts />
    </div>
  )
}

export default Home;
import React from 'react'
import TagCloud from '../widgets/TagCloud';
import NewsletterForm from '../widgets/NewsletterForm';
import RandomPosts from '../widgets/RandomPosts';

const WidgetPane = () => {
  return (
    <div className='h-100 p-4 border-start'>
      <TagCloud />
      <NewsletterForm />
      <RandomPosts />
    </div>
  )
}

export default WidgetPane;
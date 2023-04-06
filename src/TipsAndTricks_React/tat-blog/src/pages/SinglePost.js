import React from 'react'
import AuthorDetail from '../components/blog/authors/AuthorDetail';
import TagList from '../components/blog/tags/TagList';

const post = {
    "id": 11,
    "title": "Google Cloud Run",
    "shortDescription": "Google Cloud Run is a serverless container app service. You can deploy containerised apps to the cloud, which will autoscale horizontally with minimal configuration. It is an alternative to Kubernetes, but you only pay for usage. You do not pay for server uptime when there is no server usage. ",
    "description": "Containers give your app portability. If your app runs on a Linux Docker container, your app will run just about anywhere in the cloud, on-prem or on a well-speced computer. Containers decouple your app from the Cloud Host. For example, if I build a .NET Web API, it will require the .NET runtime and perhaps other dependencies. I can deploy the app to a cloud host that supports .NET, but what if they change one of the .NET dependencies? It might mean that my app becomes incompatible with their version of .NET. My app needs to be compatible with their service and not the other way around.\r\n\r\nContainerization solves this issue. I can install any version of .NET on the container and deploy it. The cloud host doesn't need to know anything about the version of .NET I am using. I can configure the container however I like. Infact, I can install any version of any runtime that suits my needs on the container. ",
    "meta": "Google Cloud Run",
    "urlSlug": "google-cloud-run",
    "imageUrl": null,
    "viewCount": 11,
    "postedDate": "2022-01-14T08:25:00",
    "modifiedDate": null,
    "category": {
        "id": 5,
        "name": "OOP",
        "urlSlug": "oop"
    },
    "author": {
        "id": 1,
        "fullName": "Jason Mouth",
        "urlSlug": "jason-mouth"
    },
    "tags": [
        {
            "id": 2,
            "name": "ASP.NET MVC",
            "urlSlug": "asp-net-mvc"
        },
        {
            "id": 9,
            "name": "MongoDB",
            "urlSlug": "mongodb"
        }
    ]
};

const SinglePost = () => {
    return (
        <>
            <div className='single-post mb-5'>
                <h1 className='mb-3'>
                    {post.title}
                </h1>

                <div className='h-25 overflow-hidden mb-4'>
                    <img
                        className='featured-image'
                        src={post.imageUrl || '/tips.jpg'}
                        alt={post.title} />
                </div>

                <div className='mb-3 d-flex justify-content-between'>
                    <div>
                        Published on:
                        <span className='fw-bold text-primary'>
                            {post.postedDate}
                        </span>
                    </div>
                    <div>
                        By :
                        <a
                            className='text-decoration-none fw-bold'
                            href={`/author/${post.author.urlSlug}`}>
                            {post.author.fullName}
                        </a>
                    </div>
                    <div>
                        Category:
                        <a
                            className='text-decoration-none fw-bold'
                            href={`/categories/${post.category.urlSlug}`}>
                            {post.category.name}
                        </a>
                    </div>
                </div>

                <div className='mb-3'>
                    <TagList tags={post.tags} />
                </div>

                <p className='fw-bold text-success'>
                    {post.shortDescription}
                </p>

                <div
                    className='post-content' 
                    dangerouslySetInnerHTML={{ __html: post.description }}>
                </div>
            </div>

            <AuthorDetail authorSlug={post.author.urlSlug} />
        </>
    )
}

export default SinglePost;
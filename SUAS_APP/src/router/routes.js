const routes = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '', component: () => import('pages/IndexPage.vue') },
      { path: '/students', component: () => import('pages/StudentsPage.vue') },
      { path: '/courses', component: () => import('pages/CoursesPage.vue') }
    ]
  },
  // {
  //   path: '/students',
  //   name: 'Students',
  //   component: StudentsPage,
  // },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue')
  }
]

export default routes

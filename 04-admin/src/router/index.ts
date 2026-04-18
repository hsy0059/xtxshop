import { createRouter, createWebHistory } from 'vue-router'
import { useUserStore } from '@/stores/user'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'Login',
      component: () => import('@/views/login/LoginView.vue'),
      meta: { public: true }
    },
    {
      path: '/',
      name: 'Layout',
      component: () => import('@/components/LayoutView.vue'),
      redirect: '/dashboard',
      children: [
        {
          path: 'dashboard',
          name: 'Dashboard',
          component: () => import('@/views/dashboard/DashboardView.vue'),
          meta: { title: '数据概览', icon: 'DataLine' }
        },
        {
          path: 'goods',
          name: 'Goods',
          component: () => import('@/views/goods/GoodsView.vue'),
          meta: { title: '商品管理', icon: 'Goods' }
        },
        {
          path: 'goods/category',
          name: 'Category',
          component: () => import('@/views/goods/CategoryView.vue'),
          meta: { title: '分类管理', icon: 'FolderOpened' }
        },
        {
          path: 'order',
          name: 'Order',
          component: () => import('@/views/order/OrderView.vue'),
          meta: { title: '订单管理', icon: 'List' }
        },
        {
          path: 'user',
          name: 'User',
          component: () => import('@/views/user/UserView.vue'),
          meta: { title: '用户管理', icon: 'User' }
        },
        {
          path: 'marketing/banner',
          name: 'Banner',
          component: () => import('@/views/marketing/BannerView.vue'),
          meta: { title: '轮播图管理', icon: 'Picture' }
        },
        {
          path: 'marketing/hot',
          name: 'Hot',
          component: () => import('@/views/marketing/HotView.vue'),
          meta: { title: '热门推荐', icon: 'Star' }
        }
      ]
    }
  ]
})

router.beforeEach((to, from, next) => {
  const userStore = useUserStore()

  if (!to.meta.public && !userStore.token) {
    next('/login')
  } else if (to.path === '/login' && userStore.token) {
    next('/')
  } else {
    next()
  }
})

export default router

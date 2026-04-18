import axios from 'axios'
import { ElMessage } from 'element-plus'
import { useUserStore } from '@/stores/user'

const request = axios.create({
  baseURL: '/api',
  timeout: 10000
})

request.interceptors.request.use(
  (config) => {
    const userStore = useUserStore()
    if (userStore.token) {
      config.headers.Authorization = `Bearer ${userStore.token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

request.interceptors.response.use(
  (response) => {
    const { data } = response
    if (data.code !== 200) {
      ElMessage.error(data.msg || '请求失败')
      if (data.code === 401) {
        const userStore = useUserStore()
        userStore.logout()
        window.location.href = '/login'
      }
      return Promise.reject(new Error(data.msg))
    }
    return data.result
  },
  (error) => {
    ElMessage.error(error.response?.data?.msg || '网络错误')
    if (error.response?.status === 401) {
      const userStore = useUserStore()
      userStore.logout()
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

export default request

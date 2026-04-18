import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { AdminUserInfo, AdminLoginResult } from '@/types'

export const useUserStore = defineStore('user', () => {
  const token = ref<string>(localStorage.getItem('token') || '')
  const userInfo = ref<AdminUserInfo | null>(null)

  const setToken = (newToken: string) => {
    token.value = newToken
    localStorage.setItem('token', newToken)
  }

  const setUserInfo = (info: AdminUserInfo) => {
    userInfo.value = info
  }

  const logout = () => {
    token.value = ''
    userInfo.value = null
    localStorage.removeItem('token')
  }

  return {
    token,
    userInfo,
    setToken,
    setUserInfo,
    logout
  }
})

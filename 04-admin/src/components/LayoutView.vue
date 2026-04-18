<template>
  <el-container class="layout">
    <el-aside width="200px" class="aside">
      <div class="logo">
        <h2>小兔鲜儿后台</h2>
      </div>
      <el-menu
        :default-active="$route.path"
        router
        background-color="#304156"
        text-color="#bfcbd9"
        active-text-color="#409EFF"
      >
        <el-menu-item index="/dashboard">
          <el-icon><DataLine /></el-icon>
          <span>数据概览</span>
        </el-menu-item>
        <el-sub-menu index="/goods">
          <template #title>
            <el-icon><Goods /></el-icon>
            <span>商品管理</span>
          </template>
          <el-menu-item index="/goods">商品列表</el-menu-item>
          <el-menu-item index="/goods/category">分类管理</el-menu-item>
        </el-sub-menu>
        <el-menu-item index="/order">
          <el-icon><List /></el-icon>
          <span>订单管理</span>
        </el-menu-item>
        <el-menu-item index="/user">
          <el-icon><User /></el-icon>
          <span>用户管理</span>
        </el-menu-item>
        <el-sub-menu index="/marketing">
          <template #title>
            <el-icon><Star /></el-icon>
            <span>营销管理</span>
          </template>
          <el-menu-item index="/marketing/banner">轮播图管理</el-menu-item>
          <el-menu-item index="/marketing/hot">热门推荐</el-menu-item>
        </el-sub-menu>
      </el-menu>
    </el-aside>
    <el-container>
      <el-header class="header">
        <div class="header-right">
          <el-dropdown @command="handleCommand">
            <span class="user-info">
              {{ userStore.userInfo?.nickname || userStore.userInfo?.username }}
              <el-icon class="el-icon--right"><arrow-down /></el-icon>
            </span>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item command="logout">退出登录</el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </div>
      </el-header>
      <el-main class="main">
        <router-view />
      </el-main>
    </el-container>
  </el-container>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'

const router = useRouter()
const userStore = useUserStore()

const handleCommand = (command: string) => {
  if (command === 'logout') {
    userStore.logout()
    router.push('/login')
  }
}
</script>

<style scoped lang="scss">
.layout {
  height: 100vh;
}

.aside {
  background-color: #304156;

  .logo {
    height: 60px;
    display: flex;
    align-items: center;
    justify-content: center;
    color: #fff;
    border-bottom: 1px solid #1f2d3d;

    h2 {
      font-size: 18px;
      font-weight: 500;
    }
  }
}

.header {
  background-color: #fff;
  box-shadow: 0 1px 4px rgba(0, 21, 41, 0.08);
  display: flex;
  align-items: center;
  justify-content: flex-end;

  .header-right {
    .user-info {
      cursor: pointer;
      color: #606266;
    }
  }
}

.main {
  background-color: #f0f2f5;
  padding: 20px;
}
</style>

<template>
  <div class="user-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>用户管理</span>
        </div>
      </template>

      <el-form :inline="true" :model="searchForm" class="search-form">
        <el-form-item label="搜索">
          <el-input v-model="searchForm.keyword" placeholder="用户名/手机号" clearable />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">搜索</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>

      <el-table :data="userList" v-loading="loading" border>
        <el-table-column type="index" width="50" />
        <el-table-column label="头像" width="80">
          <template #default="{ row }">
            <el-avatar :src="row.avatar" :size="50" />
          </template>
        </el-table-column>
        <el-table-column prop="account" label="用户名" min-width="150" />
        <el-table-column prop="nickname" label="昵称" min-width="150" />
        <el-table-column prop="mobile" label="手机号" width="150" />
        <el-table-column prop="gender" label="性别" width="80">
          <template #default="{ row }">
            {{ row.gender || '未知' }}
          </template>
        </el-table-column>
        <el-table-column prop="fullLocation" label="地区" min-width="150" show-overflow-tooltip />
        <el-table-column prop="createdAt" label="注册时间" width="180" />
        <el-table-column label="操作" width="150" fixed="right">
          <template #default="{ row }">
            <el-button type="primary" link @click="handleDetail(row)">详情</el-button>
          </template>
        </el-table-column>
      </el-table>

      <div class="pagination">
        <el-pagination
          v-model:current-page="page"
          v-model:page-size="pageSize"
          :total="total"
          :page-sizes="[10, 20, 50, 100]"
          layout="total, sizes, prev, pager, next"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
        />
      </div>
    </el-card>

    <el-dialog v-model="detailDialogVisible" title="用户详情" width="600px">
      <el-descriptions :column="2" border v-if="currentUser">
        <el-descriptions-item label="用户ID">{{ currentUser.id }}</el-descriptions-item>
        <el-descriptions-item label="用户名">{{ currentUser.account }}</el-descriptions-item>
        <el-descriptions-item label="昵称">{{ currentUser.nickname || '-' }}</el-descriptions-item>
        <el-descriptions-item label="手机号">{{ currentUser.mobile }}</el-descriptions-item>
        <el-descriptions-item label="性别">{{ currentUser.gender || '未知' }}</el-descriptions-item>
        <el-descriptions-item label="生日">{{ currentUser.birthday || '-' }}</el-descriptions-item>
        <el-descriptions-item label="职业">{{ currentUser.profession || '-' }}</el-descriptions-item>
        <el-descriptions-item label="地区">{{ currentUser.fullLocation || '-' }}</el-descriptions-item>
        <el-descriptions-item label="注册时间" :span="2">{{ currentUser.createdAt }}</el-descriptions-item>
      </el-descriptions>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { getUserList } from '@/api/user'
import type { User } from '@/types'

const loading = ref(false)
const userList = ref<User[]>([])
const page = ref(1)
const pageSize = ref(10)
const total = ref(0)

const searchForm = reactive({
  keyword: ''
})

const detailDialogVisible = ref(false)
const currentUser = ref<User | null>(null)

const fetchUserList = async () => {
  loading.value = true
  try {
    const result = await getUserList({
      page: page.value,
      pageSize: pageSize.value,
      keyword: searchForm.keyword
    })
    userList.value = result.list
    total.value = result.total
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
}

const handleSearch = () => {
  page.value = 1
  fetchUserList()
}

const handleReset = () => {
  searchForm.keyword = ''
  page.value = 1
  fetchUserList()
}

const handleDetail = (row: User) => {
  currentUser.value = row
  detailDialogVisible.value = true
}

const handleSizeChange = (val: number) => {
  pageSize.value = val
  fetchUserList()
}

const handleCurrentChange = (val: number) => {
  page.value = val
  fetchUserList()
}

onMounted(() => {
  fetchUserList()
})
</script>

<style scoped lang="scss">
.user-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .search-form {
    margin-bottom: 20px;
  }

  .pagination {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
  }
}
</style>

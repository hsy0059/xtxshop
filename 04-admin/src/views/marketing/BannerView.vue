<template>
  <div class="banner-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>轮播图管理</span>
          <el-button type="primary" @click="handleAdd">新增轮播图</el-button>
        </div>
      </template>

      <el-table :data="bannerList" v-loading="loading" border>
        <el-table-column type="index" width="50" />
        <el-table-column label="轮播图" width="200">
          <template #default="{ row }">
            <el-image
              :src="row.imgUrl"
              :preview-src-list="[row.imgUrl]"
              style="width: 150px; height: 80px"
              fit="cover"
            />
          </template>
        </el-table-column>
        <el-table-column prop="hrefUrl" label="跳转链接" min-width="250" show-overflow-tooltip />
        <el-table-column prop="type" label="类型" width="120">
          <template #default="{ row }">
            <el-tag>{{ row.type === 1 ? '首页' : '分类页' }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="sort" label="排序" width="100" />
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-tag :type="row.status === 1 ? 'success' : 'info'">
              {{ row.status === 1 ? '启用' : '禁用' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="{ row }">
            <el-button type="primary" link @click="handleEdit(row)">编辑</el-button>
            <el-button type="danger" link @click="handleDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog
      v-model="dialogVisible"
      :title="dialogTitle"
      width="500px"
      destroy-on-close
    >
      <el-form
        ref="formRef"
        :model="form"
        :rules="rules"
        label-width="100px"
      >
        <el-form-item label="图片URL" prop="imgUrl">
          <el-input v-model="form.imgUrl" placeholder="请输入图片URL" />
        </el-form-item>
        <el-form-item label="跳转链接" prop="hrefUrl">
          <el-input v-model="form.hrefUrl" placeholder="请输入跳转链接" />
        </el-form-item>
        <el-form-item label="类型" prop="type">
          <el-radio-group v-model="form.type">
            <el-radio :label="1">首页</el-radio>
            <el-radio :label="2">分类页</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="排序" prop="sort">
          <el-input-number v-model="form.sort" :min="0" />
        </el-form-item>
        <el-form-item label="状态" prop="status">
          <el-radio-group v-model="form.status">
            <el-radio :label="1">启用</el-radio>
            <el-radio :label="0">禁用</el-radio>
          </el-radio-group>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="submitLoading" @click="handleSubmit">确定</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { getBanners, createBanner, updateBanner, deleteBanner } from '@/api/marketing'
import type { Banner } from '@/types'

const loading = ref(false)
const bannerList = ref<Banner[]>([])
const dialogVisible = ref(false)
const dialogTitle = ref('新增轮播图')
const submitLoading = ref(false)
const formRef = ref()
const currentId = ref('')

const form = reactive({
  imgUrl: '',
  hrefUrl: '',
  type: 1,
  sort: 0,
  status: 1
})

const rules = {
  imgUrl: [{ required: true, message: '请输入图片URL', trigger: 'blur' }]
}

const fetchBanners = async () => {
  loading.value = true
  try {
    const result = await getBanners()
    bannerList.value = result
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
}

const handleAdd = () => {
  dialogTitle.value = '新增轮播图'
  currentId.value = ''
  form.imgUrl = ''
  form.hrefUrl = ''
  form.type = 1
  form.sort = 0
  form.status = 1
  dialogVisible.value = true
}

const handleEdit = (row: Banner) => {
  dialogTitle.value = '编辑轮播图'
  currentId.value = row.id
  form.imgUrl = row.imgUrl
  form.hrefUrl = row.hrefUrl || ''
  form.type = row.type
  form.sort = row.sort
  form.status = row.status
  dialogVisible.value = true
}

const handleDelete = async (row: Banner) => {
  try {
    await ElMessageBox.confirm('确定要删除该轮播图吗？', '提示', {
      type: 'warning'
    })
    await deleteBanner(row.id)
    ElMessage.success('删除成功')
    fetchBanners()
  } catch (error) {
    console.error(error)
  }
}

const handleSubmit = async () => {
  const valid = await formRef.value?.validate()
  if (!valid) return

  submitLoading.value = true
  try {
    if (currentId.value) {
      await updateBanner(currentId.value, form)
      ElMessage.success('修改成功')
    } else {
      await createBanner(form)
      ElMessage.success('添加成功')
    }
    dialogVisible.value = false
    fetchBanners()
  } catch (error) {
    console.error(error)
  } finally {
    submitLoading.value = false
  }
}

onMounted(() => {
  fetchBanners()
})
</script>

<style scoped lang="scss">
.banner-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>

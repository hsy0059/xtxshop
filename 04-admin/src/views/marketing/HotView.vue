<template>
  <div class="hot-page">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>热门推荐管理</span>
          <el-button type="primary" @click="handleAdd">新增推荐</el-button>
        </div>
      </template>

      <el-table :data="hotList" v-loading="loading" border>
        <el-table-column type="index" width="50" />
        <el-table-column prop="title" label="标题" min-width="150" />
        <el-table-column prop="alt" label="说明" min-width="200" show-overflow-tooltip />
        <el-table-column label="图片" width="200">
          <template #default="{ row }">
            <el-image
              v-if="row.pictures && row.pictures.length > 0"
              :src="row.pictures[0]"
              style="width: 150px; height: 80px"
              fit="cover"
            />
          </template>
        </el-table-column>
        <el-table-column prop="type" label="类型" width="120" />
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
        <el-form-item label="标题" prop="title">
          <el-input v-model="form.title" placeholder="请输入标题" />
        </el-form-item>
        <el-form-item label="说明" prop="alt">
          <el-input v-model="form.alt" placeholder="请输入说明" />
        </el-form-item>
        <el-form-item label="跳转链接" prop="target">
          <el-input v-model="form.target" placeholder="请输入跳转链接" />
        </el-form-item>
        <el-form-item label="图片URL" prop="pictures">
          <el-input
            v-model="picturesInput"
            type="textarea"
            :rows="3"
            placeholder="请输入图片URL，多个用逗号分隔"
          />
        </el-form-item>
        <el-form-item label="类型" prop="type">
          <el-input v-model="form.type" placeholder="请输入类型" />
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
import { ref, reactive, onMounted, computed } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { getHotRecommends, createHotRecommend, updateHotRecommend, deleteHotRecommend } from '@/api/marketing'
import type { HotRecommend } from '@/types'

const loading = ref(false)
const hotList = ref<HotRecommend[]>([])
const dialogVisible = ref(false)
const dialogTitle = ref('新增推荐')
const submitLoading = ref(false)
const formRef = ref()
const currentId = ref('')

const form = reactive({
  title: '',
  alt: '',
  target: '',
  pictures: [] as string[],
  type: '',
  sort: 0,
  status: 1
})

const picturesInput = computed({
  get: () => form.pictures.join(','),
  set: (val: string) => {
    form.pictures = val.split(',').filter(Boolean)
  }
})

const rules = {
  title: [{ required: true, message: '请输入标题', trigger: 'blur' }]
}

const fetchHotRecommends = async () => {
  loading.value = true
  try {
    const result = await getHotRecommends()
    hotList.value = result
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
}

const handleAdd = () => {
  dialogTitle.value = '新增推荐'
  currentId.value = ''
  form.title = ''
  form.alt = ''
  form.target = ''
  form.pictures = []
  form.type = ''
  form.sort = 0
  form.status = 1
  dialogVisible.value = true
}

const handleEdit = (row: HotRecommend) => {
  dialogTitle.value = '编辑推荐'
  currentId.value = row.id
  form.title = row.title
  form.alt = row.alt || ''
  form.target = row.target || ''
  form.pictures = row.pictures || []
  form.type = row.type
  form.sort = row.sort
  form.status = row.status
  dialogVisible.value = true
}

const handleDelete = async (row: HotRecommend) => {
  try {
    await ElMessageBox.confirm('确定要删除该推荐吗？', '提示', {
      type: 'warning'
    })
    await deleteHotRecommend(row.id)
    ElMessage.success('删除成功')
    fetchHotRecommends()
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
      await updateHotRecommend(currentId.value, form)
      ElMessage.success('修改成功')
    } else {
      await createHotRecommend(form)
      ElMessage.success('添加成功')
    }
    dialogVisible.value = false
    fetchHotRecommends()
  } catch (error) {
    console.error(error)
  } finally {
    submitLoading.value = false
  }
}

onMounted(() => {
  fetchHotRecommends()
})
</script>

<style scoped lang="scss">
.hot-page {
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
